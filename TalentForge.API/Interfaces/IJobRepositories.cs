public interface IJobRepositories
{
    Task<List<Job>> GetAllAsync();
    Task<Job?> GetByIdAsync(int id);
    Task AddAsync(Job job);
    Task UpdateAsync(Job job);
    Task DeleteAsync(int id);
}