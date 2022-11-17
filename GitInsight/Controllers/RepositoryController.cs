using GitInsight.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Octokit;
using Credentials = Octokit.Credentials;
using Repository = LibGit2Sharp.Repository;


namespace GitInsight.Controllers;


[ApiController]
public class RepositoryController: Controller
{
    
    private readonly ICloneService _cloneService;
    private string? connectionString;
    //private readonly IDatabase _database;

    public RepositoryController(ICloneService cloneService)
    {
        _cloneService = cloneService;
       // _database = database;
       var configuration = new ConfigurationBuilder()
           .AddUserSecrets<RepositoryController>()
           .Build();
       connectionString = configuration.GetConnectionString("ConnectionString");
    }


    [HttpGet]
    [Route("/{user}/{repository}")]
    
    
    public async Task<IActionResult> CloneOrFind(string user, string repository)
    {
        //safe secret 
       
        
        if (!_cloneService.FindRepositoryOnMachine(repository, out var repoPath))
        {
            repoPath = await _cloneService.CloneRepositoryFromWebAsync(user, repository);
        }

        var repo = new Repository(repoPath);
        //_database.AddRepository(repo);
        
       // var db = _database.getCommitsPrDay(repo);
       //forks test
       int count = 1000;
       var client = new GitHubClient(new ProductHeaderValue(repository));
       var tokenAuth = new Credentials(connectionString); // NOTE: not real token
       client.Credentials = tokenAuth;
       
       var request = new SearchRepositoriesRequest(repository) { Fork = ForkQualifier.IncludeForks,User = user};
       var result = await client.Search.SearchRepo(request);
       foreach (var item in result.Items)
       {
           
           count = item.ForksCount;
       }
       Console.WriteLine("{0} Forks were created --------",count);
       // Console.WriteLine("https://api.github.com/repos/{0}/{1}/forks",user,repository);
       // Console.WriteLine(result.Items);
       
        //put result of analysis into json instead of current placeholders
        return Json(new{count,user,repository,result});
    }

    // public async Task<IActionResult> GetForks(string user, string repository)
    // {
    //     var client = new GitHubClient(new ProductHeaderValue(repository));
    //     var userFork = await client.User.Get(user);
    //     Console.WriteLine("------------  {0} has {1} public repositories - go check out their profile at ??-------------",
    //         userFork.Name,
    //         userFork.PublicRepos
    //     );
    //     var request = new SearchRepositoriesRequest(repository) { Fork = ForkQualifier.IncludeForks,User = user};
    //     var result = await client.Search.SearchRepo(request);
    //     Console.WriteLine("https://api.github.com/repos/{0}/{1}/forks",user,repository);
    //     Console.WriteLine(result.Items);
    //     return Json(new { result });
    // }

    
    
}