using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class GitInsightContext: DbContext
{
    public GitInsightContext(DbContextOptions<GitInsightContext> options)
        : base(options)
    {
    }
    public virtual DbSet<AuthorEntity> Authors { get; set; }
    public virtual DbSet<CommitEntity> Commits { get; set; }
    public virtual DbSet<RepositoryEntity> Repositories { get; set; }




    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
    }
    


}