using GitInsight.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Octokit;
using Repository = LibGit2Sharp.Repository;


namespace GitInsight.Controllers;


[ApiController]
public class RepositoryController: Controller
{
    
    private readonly ICloneService _cloneService;
    //private readonly IDatabase _database;

    public RepositoryController(ICloneService cloneService)
    {
        _cloneService = cloneService;
       // _database = database;

    }


    [HttpGet]
    [Route("/{user}/{repository}")]
    
    
    public async Task<IActionResult> CloneOrFind(string user, string repository)
    {
        if (!_cloneService.FindRepositoryOnMachine(repository, out var repoPath))
        {
            repoPath = await _cloneService.CloneRepositoryFromWebAsync(user, repository);
        }

        
        var repo = new Repository(repoPath);
        // _database.AddRepository(repo);
        // var db = _database.getCommitsPrDay(repo);
        
        //forks test
        // var client = new GitHubClient(new ProductHeaderValue(repository));
        // var userFork = await client.User.Get(user);
        // Console.WriteLine("------------  {0} has {1} public repositories - go check out their profile at ??-------------",
        //     userFork.Name,
        //     userFork.PublicRepos);
        // var request = new SearchRepositoriesRequest(repository) { Fork = ForkQualifier.IncludeForks };
        // var result = await client.Search.SearchRepo(request);
        
        
        //put result of analysis into json instead of current placeholders
        return Json(new{user,repository});
        }
    
    // public async Task<IActionResult> GetForks(string user, string repository)
    // {
    //     var client = new GitHubClient(new ProductHeaderValue(repository));
    //     var userFork = await client.User.Get(user);
    //     Console.WriteLine("{0} has {1} public repositories - go check out their profile at ??",
    //         userFork.Name,
    //         userFork.PublicRepos);
    //     var request = new SearchRepositoriesRequest(repository) { Fork = ForkQualifier.IncludeForks };
    //     var result = await client.Search.SearchRepo(request);
    //     return Json(new { result });
    // }
    
}