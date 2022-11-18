using GitInsight.Core;
using Infrastructure.Entities;
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
    

    
    public IEnumerable<(int commitCount, DateTime date)> getCommitsPrDay(string RID)
    {
        var commits = _context.Commits.ToList();

        
        return commits.Where(c=>c.RID==RID).GroupBy(c => c.date.Date)
            .Select(g => (g.Count(),g.Key));
    }

    public IReadOnlyDictionary<string, IEnumerable<(int commitCount, DateTime date)>> getCommitsPrAuthor(String RID)
    {
        var commits = _context.Commits.ToList();
        var authors = commits.Where(c => c.RID==RID).Select(c=>c.Author).Distinct();

        var toReturn = new Dictionary<string, IEnumerable<(int commitCount, DateTime date)>>();

        foreach (var author in authors)
        {
             toReturn.Add(author,commits.Where(c => c.Author == author && c.RID==RID)
                 .GroupBy(i => i.date.Date)
                 .Select(g => (g.Count(), g.Key)));
        }

        return toReturn;
    }
    
}