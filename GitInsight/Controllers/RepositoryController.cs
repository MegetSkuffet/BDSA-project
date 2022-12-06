using GitInsight.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Octokit;
using Credentials = Octokit.Credentials;
using ProductHeaderValue = System.Net.Http.Headers.ProductHeaderValue;
using Repository = LibGit2Sharp.Repository;
using GitInsight.Core.Abstractions;

namespace GitInsight.Controllers;

[ApiController]
[Route("[controller]")]
public class RepositoryController : Controller
{
    private readonly ICloneService _cloneService;
    private readonly IDatabase _database;
    private string? _connectionString;

    public RepositoryController(ICloneService cloneService, IDatabase database)
    {
        _cloneService = cloneService;
        _database = database;

        //Security token
        var configuration = new ConfigurationBuilder()
            .AddUserSecrets<RepositoryController>()
            .Build();
        _connectionString = configuration.GetConnectionString("ConnectionString");
    }

    [HttpGet]
    [Route("commitfrequency/{user}/{repository}")]
    public async IAsyncEnumerable<Commit> CommitFrequencyMode(string user, string repository)
    {
        if (!_cloneService.FindRepositoryOnMachine(user, repository, out var handle))
        {
            handle = await _cloneService.CloneRepositoryFromWebAsync(user, repository);
        }

        var repo = new InsightRepository(new Repository(handle.Path));
        _database.AddRepository(repo);

        var db = _database.GetCommitsPrDay(repo);
        repo.Dispose();

        //put result of analysis into json instead of current placeholders
        foreach (var commit in db)
        {
            yield return new Commit { Date = commit.date, Count = commit.count };
        }

    }

    [HttpGet]
    [Route("Author/{user}/{repository}")]
    public async IAsyncEnumerable<Author> AuthorMode(string user, string repository)
    {
        if (!_cloneService.FindRepositoryOnMachine(user, repository, out var handle))
        {
            handle = await _cloneService.CloneRepositoryFromWebAsync(user, repository);
        }

        var repo = new InsightRepository(new Repository(handle.Path));
        _database.AddRepository(repo);

        var db = _database.GetCommitsPrAuthor(repo);
        repo.Dispose();

        //put result of analysis into json instead of current placeholders
        foreach (var author in db)
        {
            var list = new List<Commit>();

            foreach (var commit in author.Value)
            {
                list.Add(new Commit { Date = commit.date, Count = commit.commitCount });
            }

            yield return new Author { Name = author.Key, AuthorCommits = list.ToArray() };
        }
    }
    
    [HttpGet]
    [Route("forks/{user}/{repository}")]
    public async IAsyncEnumerable<Forks> GetForks(string user, string repository)
    {
        var client = new GitHubClient(new Octokit.ProductHeaderValue(repository));
        var tokenAuth = new Credentials(_connectionString); // Token created with user-secret
        client.Credentials = tokenAuth; 
        var forks = await client.Repository.Forks.GetAll(user, repository);
        var info = forks.Select(c =>new{user=c.Owner.Login,repo=c.Name,url=c.GitUrl});
        
        foreach (var item in info)
        {
            yield return new Forks(){ user = item.user,repoName = item.repo, url = item.url} ; 
        }
    }
    
    public class Commit
    {
        public DateTime Date { get; set; }

        public int Count { get; set; }
    }

    public class Author
    {
        public string Name { get; set; }

        public Commit[] AuthorCommits { get; set; }
    }

    public class Forks
    {
        public string user { get; set; }
        public string repoName { get; set; }
        public string url { get; set; }
    }
}


