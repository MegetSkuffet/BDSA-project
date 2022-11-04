using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class GitInsightContext: DbContext
{
    public GitInsightContext(DbContextOptions<GitInsightContext> options)
        : base(options)
    {
    }
    
    //Vi behøver vel egentligt slet ikke author, gør vi? Det er en weak entity type der kun har én attribute. At lave
    //et helt table til det virker skørt.
    //public virtual DbSet<AuthorEntity> Authors { get; set; }
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