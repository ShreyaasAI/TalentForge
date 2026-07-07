    using Microsoft.EntityFrameworkCore;
    
   public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){    }
    public DbSet<Candidate> Candidates{get; set;}
    public DbSet<Job> Jobs{get; set;}
    public DbSet<User> Users{get; set;}

}
