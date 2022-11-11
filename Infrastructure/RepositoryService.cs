using GitInsight.Core;
namespace Infrastructure;


public class RepositoryService : IRepositoryService
{
    private readonly GitInsightContext _context;

    public RepositoryService(GitInsightContext context)
    {
        _context = context;
    }
    
    public (Response response, string ID) Create(RepositoryCreateDTO repository)
    {
        var entity = _context.Repositories.FirstOrDefault(r => r.ID == repository.ID);
        Response res;

        if (entity is null)
        {
            entity = new RepositoryEntity()
            {
                ID = repository.ID,
                LastCommitSha = repository.lastCommitSha
            };
            _context.Repositories.Add(entity);
            _context.SaveChanges();
            res = Response.Created;
        }
        else
        {
            res = Response.Conflict;
        }

        var created = new RepositoryDTO(entity.ID, entity.LastCommitSha);
        return (res, created.ID);
    }
    

    public Response Update(RepositoryUpdateDTO repository)
    {
        var entity = _context.Repositories.FirstOrDefault(r => r.ID == repository.ID );
        Response res;

        if (entity is not null)
        {
            entity.LastCommitSha = repository.lastCommitSha;
            _context.SaveChanges();
            res = Response.Updated;
        }else if(_context.Repositories.FirstOrDefault(r=>r.ID !=repository.ID)!=null) {
            res = Response.Conflict;
        }else {
            res = Response.NotFound;
        }
        return res;
    }
}