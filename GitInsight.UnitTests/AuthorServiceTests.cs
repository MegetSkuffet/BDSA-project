using GitInsight.Core;
using Infrastructure;
using LibGit2Sharp;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace GitInsight.UnitTests;

public class AuthorServiceTests
{
    private readonly GitInsightContext _context;
    private readonly AuthorService _service;

    public AuthorServiceTests()
    {
        var connection = new SqliteConnection("DataSource=:memory:");
        connection.Open();
        var builder = new DbContextOptionsBuilder<GitInsightContext>();
        builder.UseSqlite(connection);
        var context = new GitInsightContext(builder.Options);
        context.Database.EnsureCreated();
        context.CommitsPrAuthor.Add(new AuthorEntity(){Username = "ExisitingAuthor",amountPerDay = 1,date = new DateTime(2022,01,01),RID="1"});
        context.SaveChanges();
        _context = context;
        _service = new AuthorService(_context);
    }

    #region create
    [Fact]
    public void Create_Author_returns_Created_with_author(){
        var(response,created) = _service.Create(new AuthorCreateDTO("TestAuthor1","123",new DateTime(2022,11,08),4));
        response.Should().Be(Response.Created);
        created.Should().Be((new AuthorDTO("TestAuthor1","123",new DateTime(2022,11,08),4).Username));
    }
    [Fact]
    public void Create_author_given_exisiting_author_returns_Conflict(){
        var(response,author) = _service.Create(new AuthorCreateDTO("ExisitingAuthor", "1",new DateTime(2022,01,01),1));
        response.Should().Be(Response.Conflict);
        //var exsitingAuthor = new AuthorDTO("ExisitingAuthor","123",new DateTime(2022,11,08),4);
        author.Should().Be("ExisitingAuthor");   
    }
    #endregion

    #region readAll

    [Fact]
    public void ReaddAll_Authors_sortedBy_AuthorName() {
        _context.CommitsPrAuthor.AddRange(
            new AuthorEntity(){Username = "Herman",amountPerDay = 4,date = new DateTime(2022,01,02),RID="2"},
            new AuthorEntity(){Username = "Ulla",amountPerDay = 3,date = new DateTime(2022,01,03),RID="3"});
        _context.SaveChanges();
        _service.ReadAllByAuthor().Should().BeEquivalentTo(new[]
        {
            new AuthorDTO("ExisitingAuthor", "1", new DateTime(2022, 01, 01), 1),
            new AuthorDTO("Herman", "2", new DateTime(2022, 01, 02), 4),
            new AuthorDTO("Ulla", "3", new DateTime(2022, 01, 03), 3)
        });
    }
    #endregion   
}