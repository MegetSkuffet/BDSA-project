using System.Runtime.CompilerServices;
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
    private readonly string? _connectionString;
    //private readonly IDatabase _database;

    public RepositoryController(ICloneService cloneService)
    {
        _cloneService = cloneService;
       // _database = database;
       var configuration = new ConfigurationBuilder()
           .AddUserSecrets<RepositoryController>()
           .Build();
       _connectionString = configuration.GetConnectionString("ConnectionString");
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
        var forks = await GetForks(user, repository);
        var forksTest = await GetForksTest(user, repository);
        var info = forksTest.Select(c =>new{user=c.Owner.Login,repo=c.Name,url=c.GitUrl});
        //_database.AddRepository(repo);
        // var db = _database.getCommitsPrDay(repo);
      
        //put result of analysis into json instead of current placeholders
        return Json(new{user,repository,forks,info});
    }
    
    [Route("/{user}/{repository}")]
    public async Task<IActionResult> GetForks(string user, string repository)
    {
        //forks test
        var client = new GitHubClient(new ProductHeaderValue(repository));
        var tokenAuth = new Credentials(_connectionString); // Token created with user-secret
        client.Credentials = tokenAuth;
        var forks = await client.Repository.Forks.GetAll(user, repository);
        var info = forks.Select(c =>new{user=c.Owner.Login,repo=c.Name,url=c.GitUrl});
        foreach (var item in info)
        {
            Console.WriteLine(item);
        }
        
        //put result of analysis into json instead of current placeholders
        return Json(info);
    }
    
    //Testing purposes
    [Route("/{user}/{repository}")]
    public async Task<IReadOnlyList<Octokit.Repository>> GetForksTest(string user, string repository)
    {
        //forks test
        var client = new GitHubClient(new ProductHeaderValue(repository));
        var tokenAuth = new Credentials(_connectionString); // Token created with user-secret
        client.Credentials = tokenAuth;
        var forks = await client.Repository.Forks.GetAll(user, repository);
        var info = forks.Select(c =>new{user=c.Owner.Login,repo=c.Name,url=c.GitUrl});
        foreach (var item in info)
        {
            Console.WriteLine(item);
        }
        
        //put result of analysis into json instead of current placeholders
        return forks;
    }


}