using GitInsight.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Octokit;
using ProductHeaderValue = System.Net.Http.Headers.ProductHeaderValue;
using Repository = LibGit2Sharp.Repository;


namespace GitInsight.Controllers;


[ApiController]
[Route("[controller]")]
public class RepositoryController: Controller
{
    
    private readonly ICloneService _cloneService;
    private readonly IDatabase _database;
    private string? connectionString;

    public RepositoryController(ICloneService cloneService, IDatabase database)
    {
        _cloneService = cloneService;
        _database = database;
        //Security token
        var configuration = new ConfigurationBuilder()
            .AddUserSecrets<RepositoryController>()
            .Build();
        connectionString = configuration.GetConnectionString("ConnectionString");

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
    // [HttpGet]
    // [Route("Forks/{user}/{repository}")]
    // public async IAsyncEnumerable<Forks> GetForks(string user, string repository)
    // {
    //     var client = new GitHubClient(new ProductHeaderValue(repository));
    //     var tokenAuth = new Credentials(connectionString); // NOTE: not real token
    //     client.Credentials = tokenAuth;
    //     var request = new SearchRepositoriesRequest(repository) { Fork = ForkQualifier.IncludeForks,User = user};
    //     var result = await client.Search.SearchRepo(request);
    //     foreach (var item in result.Items)
    //     {
    //        
    //         count = item.ForksCount;
    //     }
    //     Console.WriteLine("------------  {0} has {1} public repositories - go check out their profile at ??-------------",
    //         userFork.Name,
    //         userFork.PublicRepos
    //     );
    //
    //     foreach (var author in db)
    //     { 
    //         List<committest> list = new List<committest>();
    //         
    //         foreach (var commit in author.Value)
    //         {
    //             list.Add(new committest{date = commit.date, count = commit.commitCount});
    //         }
    //         yield return new Author{ name = author.Key, AuthorCommits = list.ToArray()} ; 
    //     }
    //     
    //     return Json(new { result });
    // }
    //
    
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