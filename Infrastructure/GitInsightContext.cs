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
            r.Property(p => p.ID).IsRequired();
            r.Property(p => p.lastCommitSha).IsRequired();
            r.HasIndex(p => p.ID).IsUnique();
            r.HasKey(p => p.ID);
        });

        modelBuilder.Entity<CommitEntity>(c =>
        {
            c.Property(p => p.RID).IsRequired();
            c.HasKey(p => new {p.RID,p.date});
            

        });


        modelBuilder.Entity<AuthorEntity>(A =>
        {
            A.HasKey(a => new { a.RID, a.date, a.author });
            A.Property(a => a.RID).IsRequired();
            A.Property(a => a.amountPrDay).IsRequired();
            

        });
    }
    


}