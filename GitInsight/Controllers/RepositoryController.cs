using GitInsight.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace GitInsight.Controllers;


[ApiController]
public class RepositoryController: Controller
{
    
    private readonly ICloneService _cloneService;

    public RepositoryController(ICloneService cloneService)
    {
        _cloneService = cloneService;
    }


    [Route("/{user}/{repository}")]
    public async Task<IActionResult> CloneOrFind(string user, string repository)
    {
        string repoPath;
        if (!_cloneService.FindRepositoryOnMachine(user,repository, out repoPath))
        {
            repoPath = await _cloneService.CloneRepositoryFromWebAsync(user, repository);
        }
        else
        {
            //Update the local repo
            //Do analysis of repo or get analysis from db
        }
        
        return Json(new {
            user,repository
        });
    }
    
}