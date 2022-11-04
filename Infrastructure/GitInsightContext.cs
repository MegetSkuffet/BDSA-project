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
            r.Property(p => p.ID).IsRequired();
            r.HasIndex(p => p.ID).IsUnique();
            r.HasKey(p => p.ID);
        });

        modelBuilder.Entity<CommitEntity>(c =>
        {
            c.Property(p => p.ID).IsRequired();
            c.Property(p => p.RID).IsRequired();
            c.HasKey(p => p.ID);
            c.HasIndex(p => p.ID).IsUnique();
            c.HasOne<RepositoryEntity>().WithMany().HasForeignKey(b => b.RID);

        });
    }
    


}