using Microsoft.EntityFrameworkCore;
public class JobRepository : IJobRepositories
{
    private readonly AppDbContext _context;
    public JobRepository(AppDbContext context)
    {
        _context = context;
    }
    public async Task<List<Job>> GetAllAsync()
     => await _context.Jobs.ToListAsync()  ;
    public async Task<Job?> GetByIdAsync(int id)
    => await _context.Jobs.FindAsync(id);
    public async Task AddAsync(Job job)
    {
        _context.Jobs.Add(job);
        await _context.SaveChangesAsync();
    }
    public async Task UpdateAsync(Job job)
    {
        _context.Entry(job).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }
    public async Task DeleteAsync(int id)
    {
        var job = await _context.Jobs.FindAsync(id);
        if (job != null)
        {
            _context.Jobs.Remove(job);
            await _context.SaveChangesAsync();
        }
    }
     
}

