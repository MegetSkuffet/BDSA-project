using Infrastructure.Core;
namespace Infrastructure;

public class CommitService : ICommitService{
    private readonly GitInsightContext _context;
    public CommitService(GitInsightContext context)
    {
        _context = context;
    }
    (Response Response, string CommitId) Create(CommitCreateDTO commit){
        var entity = _context.Commit.FirstOrDefault(c=>c.ID == c.ID);
        Response res;
        if(entity is null){
            entity = new Commit(){ID = commit.id}
            _context.Commit.Add(entity);
        }else{

        }
    }
    IReadOnlyCollection<CommitDTO> ReadAllByDate(){
        throw new NotImplemented();
    }

    IReadOnlyCollection<CommitDTO> ReadAllByAuthor(string Author){
        throw new NotImplemented();
    }
}