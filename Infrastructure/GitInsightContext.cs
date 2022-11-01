using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class GitInsightContext: DbContext
{
    public GitInsightContext(DbContextOptions<GitInsightContext> options)
        : base(options)
    {
    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
    }
    


}