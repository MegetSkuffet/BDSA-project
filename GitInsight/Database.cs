using GitInsight.Core;
using Infrastructure;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace GitInsight;

public class Database
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

    public void frequencyMode(Repository repository)
    {
        var repoID = getRepoID(repository);
        var latestSha = getnewestCommitSha(repository);
        if (_repositoryService.checkLatestSha(new RepositoryUpdateDTO(repoID, latestSha)))
        {
            //Same sha, read from db instead of writing
            Console.WriteLine("Reading instead of writing");
        } else {
            Console.WriteLine("Writing instead of reading");
            _repositoryService.Create(new RepositoryCreateDTO(repoID, latestSha));
        
            addCommitsFrequencyMode(repository.Commits.ToList(),repoID);

        }
        Console.WriteLine(_context.Repositories.Count());
        Console.WriteLine(_context.CommitsPrDay.Count());
    }

    private string getRepoID(Repository r)
    {
        return r.Commits.ToList()[0].Sha;
    }

    private string getnewestCommitSha(Repository r)
    {
        return r.Commits.ToList()[r.Commits.ToList().Count - 1].Sha;
    }

    private void addCommitsFrequencyMode(IList<Commit> commits, string ID)
    {
        var groupedBy = commits.GroupBy(c => c.Author.When.Date);
        foreach (var c in groupedBy)
        {
            _commitService.Create(new CommitDTO(ID, c.Key, c.Count()));
        }
    }

    public IReadOnlyCollection<CommitDTO> getAllCommits()
    {
        return _commitService.ReadAllCommits();
    }
    
    public GitInsightContext Context => _context;
}