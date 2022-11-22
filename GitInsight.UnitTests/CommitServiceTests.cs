using GitInsight.Core;
using Infrastructure;
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
        var response = _service.Create(new CommitDTO("ExmapleRepoID","ExampleCommitID","AuthorName",new DateTimeOffset(1,1,1,1,1,1,1,TimeSpan.Zero)));
        response.Response.Should().Be(Response.Created);
        _context.Commits.Count().Should().Be(1);
    }

    [Fact]
    public void create_with_same_RID_and_CID_should_return_conflict()
    {
        var response = _service.Create(new CommitDTO("ExmapleRepoID","ExampleCommitID","AuthorName",new DateTimeOffset(1,1,1,1,1,1,1,TimeSpan.Zero)));
        var response2 = _service.Create(new CommitDTO("ExmapleRepoID","ExampleCommitID","AuthorName",new DateTimeOffset(1,1,1,1,1,1,1,TimeSpan.Zero)));
        response.Response.Should().Be(Response.Created);
        response2.Response.Should().Be(Response.Conflict);
    }

    [Fact]
    public void create_several_and_read_all_should_return_all()
    {
        var response1 = _service.Create(new CommitDTO("ExmapleRepoID","ExampleCommitID1","AuthorName",new DateTimeOffset(1,1,1,1,1,1,1,TimeSpan.Zero)));
        var response2 = _service.Create(new CommitDTO("ExmapleRepoID","ExampleCommitID2","AuthorName",new DateTimeOffset(1,1,1,1,1,1,1,TimeSpan.Zero)));
        var response3 = _service.Create(new CommitDTO("ExmapleRepoID","ExampleCommitID3","AuthorName",new DateTimeOffset(1,1,1,1,1,1,1,TimeSpan.Zero)));

        _context.Commits.Count().Should().Be(3);
    }

    [Fact]
    public void using_actual_repository()
    {
        var repository = Substitute.For<IInsightRepository>();
       var commits = repository.Commits.ToList();
       var commits2 = commits.GroupBy(c => c.Author.When.Date);
       foreach (var commit in commits2)
       {
           var response = _service.Create(new CommitDTO("ExmapleRepoID","ExampleCommitID","AuthorName",new DateTimeOffset(1,1,1,1,1,1,1,TimeSpan.Zero)));
       }


       _service.GetAllCommits().Count().Should().Be(1);
       var list = _service.GetAllCommits().ToList();
       var expectedDate = new DateTimeOffset(1, 1, 1, 1, 1, 1, 1, TimeSpan.Zero);
       list[0].date.Should().Be(expectedDate);

    }
}