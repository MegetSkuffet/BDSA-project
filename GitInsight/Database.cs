using GitInsight.Core;
using Infrastructure;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace GitInsight;

public class Database
{
    private readonly AuthorService _authorService;
    private readonly CommitService _commitService;
    private readonly RepositoryService _repositoryService;
    private readonly GitInsightContext _context;
    
    public Database()
    {
        var connection = new SqliteConnection("DataSource=:memory:");
        connection.Open();
        var builder = new DbContextOptionsBuilder<GitInsightContext>();
        builder.UseSqlite(connection);
        var context = new GitInsightContext(builder.Options);
        context.Database.EnsureCreated();
        context.SaveChanges();
        _context = context;
        _commitService = new CommitService(_context);
        _authorService = new AuthorService(_context);
        _repositoryService = new RepositoryService(_context);
    }

    public void frequency(string RID, string LatestSha, IList<Commit> list)
    {
        
    }


    public bool checkLatestSha(string RID, string latestSha)
    {
        
    }
}