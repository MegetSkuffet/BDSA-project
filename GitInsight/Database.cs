using GitInsight.Core.Abstractions;
using Microsoft.Data.Sqlite;

namespace GitInsight;

public class Database : IDatabase
{
    private readonly ICommitService _commitService;
    private readonly IRepositoryService _repositoryService;
    private readonly GitInsightContext _context;
    
    public Database(GitInsightContext context)
    {
        _context = context;
        _commitService = new CommitService(_context);
        _repositoryService = new RepositoryService(_context);
    }

    /// <summary>
    /// Adds all commits for a given repository to the database. Uses the method checkLatestSha in repositoryService to check if the latest commit is already in the database,
    /// and reads from the database instead of writing to it if it is.
    /// </summary>
    /// <param name="repository">The IRepository to be used, created with a path to the repository.</param>
    /// <returns>void</returns>
    public void AddRepository(IInsightRepository repository)
    {
        var repoId = GetRepoId(repository);
        var latestSha = GetNewestCommitSha(repository);
        if (!_repositoryService.CheckLatestSha(new RepositoryUpdateDto(repoId, latestSha)))
        {
            //Different sha, write repo entity into repo DBset and write all commit entities into commitsprday DBset
            var response = _repositoryService.Create(new RepositoryCreateDto(repoId, latestSha));
            if (response.response == Response.Conflict)
            {
                //RepoID already in DB, change existing entity sha to new latestsha.
                _repositoryService.Update(new RepositoryUpdateDto(repoId, latestSha));
            }
            AddCommits(repository);
        }
    }

    /// <summary>
    /// Gets the SHA of the first commit in the repository to be used as unique identifier for repository.
    /// </summary>
    /// <param name="r">The IRepository to be used, created with a path to the repository.</param>
    /// <returns>The SHA as a string</returns>
    private string GetRepoId(IInsightRepository r)
    {
        //Returns SHA of very first commit on repo, should be unique
        return r.Commits.ToList()[0].Sha;
    }
    /// <summary>
    /// Gets the sha of the newest commit in the repository.
    /// </summary>
    /// <param name="r">The IRepository to be used, created with a path to the repository.</param>
    /// <returns>The SHA as a string</returns>
    
    private string GetNewestCommitSha(IInsightRepository r)
    {
        //Returns SHA of latest commit on the repo
        return r.Commits.ToList()[r.Commits.ToList().Count - 1].Sha;
    }

    /// <summary>
    /// Method only used by <c>AddRepository</c> method to add all commits for a given repository to the database.
    /// </summary>
    /// <param name="repository">The IRepository to be used, created with a path to the repository.</param>
    private void AddCommits(IInsightRepository repository)
    {
        var commits = repository.Commits;
        var RID = GetRepoId(repository);
        foreach (var v in commits)
        {
            _commitService.Create(new CommitDTO(RID, v.Sha, v.Author.Name, v.Author.When.DateTime));
        }
    }

    /// <summary>
    /// Retrieves all commits for a given repository from the database grouped by day.
    /// </summary>
    /// <param name="repository">The IRepository to be used, created with a path to the repository.</param>
    /// <returns>An IEnumerable<string> containing strings in a format of "-amount- -date-"</returns>
    public IEnumerable<(int count, DateTime date)> GetCommitsPrDay(IInsightRepository repository)
    {
        var repoId = GetRepoId(repository);
        return _commitService.GetCommitsPrDay(repoId);
    }

    /// <summary>
    /// Retrieves all commits for a given repository from the database grouped by author.
    /// </summary>
    /// <param name="repository">The IRepository to be used, created with a path to the repository.</param>
    /// <returns>An IEnumerable(string author, IEnumerable(string)) containing tuples of Authors (String) and IEnumerables containing strings in a format of "-amount- -date-"</returns>
    public IReadOnlyDictionary<string, IEnumerable<(int commitCount, DateTime date)>> GetCommitsPrAuthor(
        IInsightRepository repository)
    {
        var repoId = GetRepoId(repository);
        return _commitService.GetCommitsPrAuthor(repoId);
    }

    //Only used for unit testing purposes
    public GitInsightContext Context => _context;
}