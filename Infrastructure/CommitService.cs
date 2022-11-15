using GitInsight.Core;
using LibGit2Sharp;

namespace Infrastructure;

public class CommitService : ICommitService
{
    private readonly GitInsightContext _context;
    public CommitService(GitInsightContext context)
    {
        _context = context;
    }
    
    public (Response Response, string CommitId, DateTimeOffset date) Create(CommitDTO commit)
    {
        var entity = _context.Commits.FirstOrDefault(c => c.RID == commit.RID && c.CID == commit.CID);
        Response res;
        if (entity is null)
        {
            entity = new CommitEntity() { RID = commit.RID, date = commit.date, CID = commit.CID, Author = commit.AuthorName};
            _context.Commits.Add(entity);
            _context.SaveChanges();
            res = Response.Created;
        }
        else
        {
            Console.WriteLine("Commit already exists");
            res = Response.Conflict;
        }

        var created = new CommitDTO(entity.CID, entity.RID, entity.Author, entity.date);
        return (res, created.RID, created.date);
    }
    public IReadOnlyCollection<CommitDTO> GetAllCommits()
    {
        IReadOnlyCollection<CommitDTO> commits = _context.Commits.Select(c => new CommitDTO(c.CID, c.RID, c.Author, c.date)).ToList();

        return commits;
    }
    

    
    public IEnumerable<string> getCommitsPrDay(string RID)
    {
        var commits = _context.Commits.ToList();

        return commits.GroupBy(c => c.date.Date).Select(g => $"{g.Count(),7} {g.Key:dd-MM-yy}");
    }

    public IEnumerable<(string author, IEnumerable<string>)> getCommitsPrAuthor(String RID)
    {
        var commits = _context.Commits.ToList();
        var authors = commits.Select(c => c.Author).Distinct();
        
        foreach (var author in authors)
        {
            yield return (author, commits.Where(c => c.Author == author).GroupBy(i => i.date.Date).Select(g => $"{g.Count(),7} {g.Key:dd-MM-yy}"));

        }
    }
    
}