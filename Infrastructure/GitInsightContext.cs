using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class GitInsightContext: DbContext
{
    public GitInsightContext(DbContextOptions<GitInsightContext> options)
        : base(options)
    {
    }
    

    public virtual DbSet<CommitEntity> Commits { get; set; }
    public virtual DbSet<RepositoryEntity> Repositories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
        modelBuilder.Entity<RepositoryEntity>(r =>
        {
            r.HasKey(p => p.ID);
            r.Property(p => p.LastCommitSha).IsRequired();
            
        });

        modelBuilder.Entity<CommitEntity>(c =>
        {
            c.HasKey(p => new {p.RID,p.CID});
        });
        
    }
    


}