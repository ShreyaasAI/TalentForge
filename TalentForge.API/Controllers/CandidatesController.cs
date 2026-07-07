using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.IO;
using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using Hangfire.AspNetCore;
using Hangfire.SqlServer;
using Hangfire;
using  Microsoft.AspNetCore.SignalR;
using System.Numerics;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CandidatesController : ControllerBase
{
private readonly ICandidateRepository _repository;
private readonly IDistributedCache _cache;
private readonly IHubContext<NotificationHub> _hub;
private readonly EmbeddingServices _embedding;
public CandidatesController(ICandidateRepository repository, IDistributedCache cache, IHubContext<NotificationHub> hub, EmbeddingServices embedding)
{
    _repository = repository;
    _cache = cache;
    _hub = hub;
    _embedding = embedding;
}   



[HttpGet]
public async Task<ActionResult<List<Candidate>>> GetAll()
{
    var cached = await _cache.GetStringAsync("candidates");
    if (cached != null)
        return JsonSerializer.Deserialize<List<Candidate>>(cached)!;

    var candidates = await _repository.GetAllAsync();
    await _cache.SetStringAsync("candidates", JsonSerializer.Serialize(candidates),
        new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5) });

    return candidates;
}
[HttpGet("{id}")]
public async Task<ActionResult<Candidate>> GetById(int id)
{
    var candidate = await _repository.GetByIdAsync(id);
    if (candidate == null) return NotFound();
    return candidate;
}

[HttpPost]
public async Task<ActionResult<Candidate>> Create(Candidate candidate)
{
    
    
    var vector = await _embedding.GetEmbeddingAsync($"{candidate.Name} with {candidate.YearsofExperience} years experience");
    candidate.EmbeddingJson = JsonSerializer.Serialize(vector);
    await _repository.AddAsync(candidate);
    await _cache.RemoveAsync("candidates");
    BackgroundJob.Enqueue(() => Console.WriteLine($"Welcome email queued for {candidate.Email}"));
    await _hub.Clients.All.SendAsync("NewCandidate", candidate.Name);
    return CreatedAtAction(nameof(GetById), new { id = candidate.Id }, candidate);
}

[HttpPut("{id}")]
public async Task<IActionResult> Update(int id, Candidate candidate)
{
    if (id != candidate.Id) return BadRequest();
    var exists = await _repository.GetByIdAsync(id);
    if (exists == null) return NotFound();
    await _repository.UpdateAsync(candidate);
    await _cache.RemoveAsync("candidates");
    return NoContent();
}
[Authorize(Roles = "Admin")]
[HttpDelete("{id}")]
public async Task<IActionResult> Delete(int id)
{
    await _repository.DeleteAsync(id);
    return NoContent();
}


[HttpPost("{id}/resume")]
public async Task<IActionResult> Resume(int id, IFormFile file)
{
    var candidate = await _repository.GetByIdAsync(id);
    if (candidate == null) return NotFound();

    var folder = Path.Combine("Uploads", "Resumes");
    Directory.CreateDirectory(folder);
    var fileName = $"{id}_{file.FileName}";
    var path = Path.Combine(folder, fileName);

    using var stream = new FileStream(path, FileMode.Create);
    await file.CopyToAsync(stream);

    candidate.ResumeUrl = path;
    await _repository.UpdateAsync(candidate);
    await _cache.RemoveAsync("candidates");

    return Ok(candidate.ResumeUrl);
}

[HttpGet("{candidateId}/match/{jobId}")]
public async Task<IActionResult> Match(int candidateId, int jobId, [FromServices] AppDbContext context)
    {
        var candidate = await _repository.GetByIdAsync(candidateId);
        var job = await context.Jobs.FindAsync(jobId);
        if(candidate?.EmbeddingJson == null || job?.EmbeddingJson == null) return NotFound();
        var vecA = JsonSerializer.Deserialize<float[]>(candidate.EmbeddingJson);
        var vecB = JsonSerializer.Deserialize<float[]>(job.EmbeddingJson);
        float dot=0, magA=0, magB=0;
        for(int i = 0; i < vecA.Length; i++)
        {
            dot += vecA[i] * vecB[i];
            magA += vecA[i] * vecA[i];
            magB += vecB[i] * vecB[i];
        }
        var score = dot/(MathF.Sqrt(magA) * MathF.Sqrt(magB));
        return Ok(new {candidate = candidate.Name, job = job.Title, mathScore = score});


    }
}
