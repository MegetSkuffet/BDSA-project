using GitInsight.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Octokit;
using Credentials = Octokit.Credentials;
using ProductHeaderValue = System.Net.Http.Headers.ProductHeaderValue;
using Repository = LibGit2Sharp.Repository;


namespace GitInsight.Controllers;


[ApiController]
[Route("[controller]")]
public class RepositoryController: Controller
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
    public async IAsyncEnumerable<committest> Commitfrequencymode(string user, string repository)
    {
        
        if (!_cloneService.FindRepositoryOnMachine(repository, out var repoPath))
        {
            repoPath = await _cloneService.CloneRepositoryFromWebAsync(user, repository);
          
        }

        var repo = new Repository(repoPath);
        _database.AddRepository(repo);
        
        
        var db = _database.getCommitsPrDay(repo);
        
        //put result of analysis into json instead of current placeholders
        foreach (var commit in db)
        {
            yield return new committest { date = commit.date, count = commit.count } ; 
        }
    }
    
    
    [HttpGet]
    [Route("Author/{user}/{repository}")]
    public async IAsyncEnumerable<Author> Authormode(string user, string repository)
    {
        
        if (!_cloneService.FindRepositoryOnMachine(repository, out var repoPath))
        {
            repoPath = await _cloneService.CloneRepositoryFromWebAsync(user, repository);
          
        }

        var repo = new Repository(repoPath);
        _database.AddRepository(repo);
        
        
        var db = _database.getCommitsPrAuthor(repo);
        
        //put result of analysis into json instead of current placeholders
        foreach (var author in db)
        { 
            List<committest> list = new List<committest>();
            
            foreach (var commit in author.Value)
            {
                list.Add(new committest{date = commit.date, count = commit.commitCount});
            }
            yield return new Author{ name = author.Key, AuthorCommits = list.ToArray()} ; 
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
    
    
    public class committest
    {
        public DateTime date { get; set; }
        public int count { get; set; }
    }
    
    public class Author
    {
        public string name { get; set; }
        public committest[] AuthorCommits { get; set; }
    }
    public class Forks
    {
        public string user { get; set; }
        public string repoName { get; set; }
        public string url { get; set; }
    }
}