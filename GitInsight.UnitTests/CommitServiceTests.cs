using Infrastructure;
using LibGit2Sharp;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace GitInsight.UnitTests;


public class CommitServiceTests
{
    private readonly GitInsightContext _context;
    private readonly CommitService _service;

    public CommitServiceTests()
    {
        var connection = new SqliteConnection("DataSource=:memory:");
        connection.Open();
        var builder = new DbContextOptionsBuilder<GitInsightContext>();
        builder.UseSqlite(connection);
        var context = new GitInsightContext(builder.Options);
        context.Database.EnsureCreated();
        
        context.SaveChanges();
        _context = context;
        _service = new CommitService(_context);
    }
}