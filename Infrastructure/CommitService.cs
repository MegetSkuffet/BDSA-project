using Infrastructure.Core;
namespace Infrastructure;

public class CommitService : ICommitService
{
    private readonly GitInsightContext _context;
    public CommitService(GitInsightContext context)
    {
        _context = context;
    }
    (Response Response, string CommitId) Create(CommitCreateDTO commit)
    {
        var entity = _context.CommitsPrDay.FirstOrDefault(c => c.RID == commit.RID);
        Response res;
        if (entity is null)
        {
            entity = new Commit() { RID = commit.RID, };
            _context.CommitsPrDay.Add(entity);
            _context.CommitsPrAuthor.SaveChanges();
            res = Response.Created;
        }
        else
        {
            res = Response.Conflict;
        }

        var created = new CommitDTO(entity.RID, entity.date, entity.CommitsPrDay);
        return (res, created);
    }
    IReadOnlyCollection<CommitDTO> ReadAllByDate()
    {

    }

    IReadOnlyCollection<CommitDTO> ReadAllByAuthor(string Author)
    {
        throw new NotImplemented();
    }
}