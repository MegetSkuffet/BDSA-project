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
    public (Response Response, string CommitId, DateTime date) Create(CommitDTO commit)
    {
        var entity = _context.CommitsPrDay.FirstOrDefault(c => c.RID == commit.RID && c.date == commit.date);
        Response res;
        if (entity is null)
        {
            entity = new CommitEntity() { RID = commit.RID, date = commit.date, amountPrDay = commit.amountPrDay};
            _context.CommitsPrDay.Add(entity);
            _context.SaveChanges();
            res = Response.Created;
        }
        else
        {
            res = Response.Conflict;
        }

        var created = new CommitDTO(entity.RID, entity.date, entity.amountPrDay);
        return (res, created.RID, created.date);
    }
    public IReadOnlyCollection<CommitDTO> ReadAllCommits()
    {
        throw new NotImplementedException();
    }
    
}