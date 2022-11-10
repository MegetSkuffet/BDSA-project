using GitInsight.Core;
using Infrastructure;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace GitInsight;

public class Database : IDatabase
{
    private readonly AuthorService _authorService;
    private readonly CommitService _commitService;
    private readonly RepositoryService _repositoryService;
    private readonly GitInsightContext _context;
    
    public Database()
    {
        var connection = new SqliteConnection("DataSource=:memory:");
        connection.Open();
        var builder = new DbContextOptionsBuilder<GitInsightContext>();
        builder.UseSqlite(connection);
        var context = new GitInsightContext(builder.Options);
        context.Database.EnsureCreated();
        context.SaveChanges();
        _context = context;
        _commitService = new CommitService(_context);
        _authorService = new AuthorService(_context);
        _repositoryService = new RepositoryService(_context);
    }

    public void frequencyMode(IRepository repository)
    {
        var repoId = GetRepoId(repository);
        var latestSha = GetnewestCommitSha(repository);
        if (!_repositoryService.checkLatestSha(new RepositoryUpdateDTO(repoId, latestSha)))
        {
            //Different sha, write repo entity into repo DBset and write all commit entities into commitsprday DBset
            var response = _repositoryService.Create(new RepositoryCreateDTO(repoId, latestSha));
            
            if (response.response == Response.Conflict)
            {
                //RepoID already in DB, change existing entity sha to new latestsha.
                _repositoryService.Update(new RepositoryUpdateDTO(repoId, latestSha));
            }
        
            AddCommitsFrequencyMode(repository.Commits.ToList(),repoId);
        }
    }

    private string GetRepoId(IRepository r)
    {
        //Returns SHA of very first commit on repo, should be unique
        return r.Commits.ToList()[0].Sha;
    }

    private string GetnewestCommitSha(IRepository r)
    {
        //Returns SHA of latest commit on the repo
        return r.Commits.ToList()[r.Commits.ToList().Count - 1].Sha;
    }

    private void AddCommitsFrequencyMode(IList<Commit> commits, string id)
    {
        //Adds all entities into commitsPrDay DBset for said repo. Only done if latest sha of repo is different from local sha of repo.
        var groupedBy = commits.GroupBy(c => c.Author.When.Date);
        foreach (var c in groupedBy)
        {
            _commitService.Create(new CommitDTO(id, c.Key, c.Count()));
        }
    }

    public IReadOnlyCollection<CommitDTO> getAllCommits(IRepository repository)
    {
        var repoId = GetRepoId(repository);
        return _commitService.GetCommitsByRID(repoId);
    }
    
    //Only used for unit testing purposes
    public GitInsightContext Context => _context;
}