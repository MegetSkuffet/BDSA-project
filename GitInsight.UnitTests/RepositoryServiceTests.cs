using GitInsight.Core;
using Infrastructure;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace GitInsight.UnitTests;

public class RepositoryServiceTests
{
    private readonly GitInsightContext _context;
    private readonly RepositoryService _service;

    public RepositoryServiceTests()
    {
        var connection = new SqliteConnection("DataSource=:memory:");
        connection.Open();
        var builder = new DbContextOptionsBuilder<GitInsightContext>();
        builder.UseSqlite(connection);
        var context = new GitInsightContext(builder.Options);
        context.Database.EnsureCreated();
        context.Repositories.Add(new RepositoryEntity(){ID ="1",LastCommitSha = "1"});
        context.SaveChanges();
        _context = context;
        _service = new RepositoryService(_context);
    }

    #region create
    [Fact]
    public void Create_repo_returns_Created_with_repo(){
        var (response,created) = _service.Create(new RepositoryCreateDTO("2","CreatedSha"));
        response.Should().Be(Response.Created);
        created.Should().Be((new RepositoryDTO("2","CreatedSha")).ID);
    }
    [Fact]
    public void Create_repo_given_exisiting_repo_returns_Conflict(){
        var(response,author) = _service.Create(new RepositoryCreateDTO("1", "1"));
        response.Should().Be(Response.Conflict);
        author.Should().Be("1");   
    }
    #endregion

    #region update
    [Fact]
    public void Update_repo_returns_Updated_with_repo(){
        var response = _service.Update(new RepositoryUpdateDTO("1","updatedSha"));
        response.Should().Be(Response.Updated);
        var entity = _context.Repositories.Find("1")!;
        entity.ID.Should().Be("1");
        entity.LastCommitSha.Should().Be("updatedSha");
    }
    
    [Fact]
    public void Update_given_non_existing_Repo_returns_NotFound() => _service.Update(new RepositoryUpdateDTO("42","ShaHere")).Should().Be(Response.NotFound);

    #endregion
  
}
