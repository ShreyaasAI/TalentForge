using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
[ApiController]
[Route("api/[controller]")]
public class JobsController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly EmbeddingServices _embedding;
    public JobsController(AppDbContext context, EmbeddingServices embedding)
    {
        _context = context;
        _embedding = embedding;
    }
    
    [HttpGet]
    public async Task<ActionResult<List<Job>>> GetAll()
    {
        return await _context.Jobs.ToListAsync();
    }
    [HttpGet("{id}")]
    public async Task<ActionResult<Job>> GetById(int id)
    {
        var job = await _context.Jobs.FindAsync(id);
        if(job==null) return NotFound();
        return job;
    }
    [HttpPost]
    public async Task<ActionResult<Job>> Create(Job job)
    {
        _context.Jobs.Add(job);
        var vector = await _embedding.GetEmbeddingAsync(job.Title);
        job.EmbeddingJson = JsonSerializer.Serialize(vector);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new {id= job.Id}, job);
    }
    [HttpPut("{id}")]
    public async Task<ActionResult<Job>> Update(int id, Job job)
    {
        if(id!= job.Id) return BadRequest();
        var exists = await _context.Jobs.AnyAsync(j=> j.Id==id);
        if(!exists) return NotFound();
        _context.Entry(job).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var job = await _context.Jobs.FindAsync(id);
        if(job==null) return NotFound();
        _context.Jobs.Remove(job);
        await _context.SaveChangesAsync();
        return NoContent();
    }

}