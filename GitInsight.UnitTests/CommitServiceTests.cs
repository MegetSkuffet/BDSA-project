using GitInsight.Core;
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

    [Fact]
    public void Create_should_return_response_Created()
    {
        var response = _service.Create(new CommitDTO("ExampleRepoID", new DateTime(10, 10, 10), 5));
        response.Response.Should().Be(Response.Created);
        _service.ReadAllCommits().Count.Should().Be(1);
    }

    [Fact]
    public void create_with_same_date_and_id_should_return_conflict()
    {
        var response = _service.Create(new CommitDTO("ExampleRepoID", new DateTime(10, 10, 10), 5));
        var response2 = _service.Create(new CommitDTO("ExampleRepoID", new DateTime(10, 10, 10), 5));
        response.Response.Should().Be(Response.Created);
        response2.Response.Should().Be(Response.Conflict);
    }

    [Fact]
    public void create_several_and_read_all_should_return_all()
    {
        _service.Create(new CommitDTO("ExampleRepoID", new DateTime(10, 10, 10), 5));
        _service.Create(new CommitDTO("ExampleRepoID", new DateTime(11, 11, 11), 5));
        _service.Create(new CommitDTO("ExampleRepoID", new DateTime(12, 12, 12), 5));

        _service.ReadAllCommits().Count.Should().Be(3);
    }

    [Fact]
    public void using_actual_repository()
    {
        var repository = new Repository(@"C:\Users\johan\OneDrive\Desktop\3.Semester\BDSA\assignment3_bdsa\Assignment_03");
       var commits = repository.Commits.ToList();
       var commits2 = commits.GroupBy(c => c.Author.When.Date);
       foreach (var commit in commits2)
       {
           _service.Create(new CommitDTO("ExampleRID", commit.Key, commit.Count()));
       }
      

       _service.ReadAllCommits().Count.Should().BeGreaterThan(1);
       var list = _service.ReadAllCommits().ToList();
       list[0].date.Should().Be(new DateTime(2022, 9, 23));
       list[0].amountPrDay.Should().Be(7);

    }
}