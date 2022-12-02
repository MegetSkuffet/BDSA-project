using GitInsight.Core.Abstractions;
using GitInsight.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace GitInsight.Controllers;

[ApiController]
[Route("[controller]")]
public class RepositoryController : Controller
{
    private readonly ICloneService _cloneService;
    private readonly IDatabase _database;

    public RepositoryController(ICloneService cloneService, IDatabase database)
    {
        _cloneService = cloneService;
        _database = database;
    }

    [HttpGet]
    [Route("commitfrequency/{user}/{repository}")]
    public async IAsyncEnumerable<Commit> CommitFrequencyMode(string user, string repository)
    {
        if (!_cloneService.FindRepositoryOnMachine(user, repository, out var handle))
        {
            handle = await _cloneService.CloneRepositoryFromWebAsync(user, repository);
        }

        var repo = new Repository(handle.Path);
        _database.AddRepository(repo);

        var db = _database.getCommitsPrDay(repo);

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


        var repo = new Repository(handle.Path);

        _database.AddRepository(repo);

        var db = _database.getCommitsPrAuthor(repo);

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
}
