using GitInsight.Core.Abstractions;
using GitInsight.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace GitInsight.Controllers;


[ApiController]
public class RepositoryController: Controller
{
    
    private readonly ICloneService _cloneService;
    private readonly IDatabase _database;

    public RepositoryController(ICloneService cloneService, IDatabase database)
    {
        _cloneService = cloneService;
        _database = database;

    }


    [HttpGet]
    [Route("/{user}/{repository}")]
    public async Task<IActionResult> CloneOrFind(string user, string repository)
    {
        if (!_cloneService.FindRepositoryOnMachine(repository, out var repoPath))
        {
            repoPath = await _cloneService.CloneRepositoryFromWebAsync(user, repository);
        }

        var repo = new InsightRepository(new Repository(repoPath));
        _database.AddRepository(repo);
        
        
        //put result of analysis into json instead of current placeholders
        return Json(new {
            user,repository
        });
    }
    
}