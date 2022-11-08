using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class GitInsightContext: DbContext
{
    public GitInsightContext(DbContextOptions<GitInsightContext> options)
        : base(options)
    {
    }
    

    public virtual DbSet<CommitEntity> CommitsPrDay { get; set; }
    public virtual DbSet<RepositoryEntity> Repositories { get; set; }
    public virtual DbSet<AuthorEntity> CommitsPrAuthor { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
        modelBuilder.Entity<RepositoryEntity>(r =>
        {
            r.HasKey(p => p.ID);
            r.Property(p => p.LastCommitSha).IsRequired();
            
        });

        modelBuilder.Entity<CommitEntity>(c =>
        {
            c.HasKey(p => new {p.RID,p.date});
            c.Property(p => p.amountPrDay).IsRequired();
        });


        modelBuilder.Entity<AuthorEntity>(A =>
        {
            A.HasKey(a => new { a.RID, a.date, a.Username });
            A.Property(a => a.amountPerDay).IsRequired();
        });
    }
    


}