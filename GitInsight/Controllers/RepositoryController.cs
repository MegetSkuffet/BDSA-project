using GitInsight.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace GitInsight.Controllers;


[ApiController]
public class RepositoryController: Controller
{
    
    private readonly ICloneService _cloneService;
    private readonly ICommitFrequencyService _frequencyService;

    public RepositoryController(ICloneService cloneService, ICommitFrequencyService frequencyService)
    {
        _cloneService = cloneService;
        _frequencyService = frequencyService;
    }


    [HttpGet]
    [Route("/{user}/{repository}")]
    public async Task<IActionResult> CloneOrFind(string user, string repository)
    {
        if (!_cloneService.FindRepositoryOnMachine(repository, out var repoPath))
        {
            repoPath = await _cloneService.CloneRepositoryFromWebAsync(user, repository);
        }

        IInsightRepository repo = new InsightRepository(new Repository(repoPath));

        
        var analysis = _frequencyService.GetAll(repo);
            //Update the local repo
            //Do analysis of repo or get analysis from db
        
        
        return Json(new {
            user,repository
        });
    }
    
}