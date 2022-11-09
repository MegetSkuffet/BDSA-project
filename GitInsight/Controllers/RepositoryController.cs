using Microsoft.AspNetCore.Mvc;

namespace GitInsight.Controllers;


[ApiController]
public class RepositoryController: Controller
{
    //private readonly IFooService _fooService; 
    
    //Call some service to clone or pull repo from route

    [Route("/{user}/{repository}")]
    public async Task<IActionResult> Bar(string user, string repository)
    {
        return Json(new {
            user,repository
        });
    }
    
}