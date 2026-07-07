using Microsoft.EntityFrameworkCore;
using Xunit;


 public class CandidateRepositoryTest{
    private AppDbContext GetinMemoryContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
        .UseInMemoryDatabase(Guid.NewGuid().ToString())
        .Options;
        return new AppDbContext(options);
    }
    [Fact]
    public async Task AddAsync_SavesCandidate()
    {
        var context = GetinMemoryContext();
        var repo = new CandidateRepository(context);
        var candidate = new Candidate(1, "Test", "test@x.com", 3);

        await repo.AddAsync(candidate);
        var result = await repo.GetAllAsync();

        Assert.Single(result);
        Assert.Equal("Test", result[0].Name);
    }
    [Fact]
    public async Task DeleteAsync_RemovesCandidate()
    {
        var context = GetinMemoryContext();
        var repo = new CandidateRepository(context);
        var candidate = new Candidate(1, "Test", "test@x.com", 3);
        await repo.AddAsync(candidate);

        await repo.DeleteAsync(candidate.Id);
        var result = await repo.GetAllAsync();

        Assert.Empty(result);
}

}