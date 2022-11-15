using GitInsight.Core;
using Infrastructure.Entities;

namespace Infrastructure;


public class RepositoryService : IRepositoryService
{
    private readonly GitInsightContext _context;

    public RepositoryService(GitInsightContext context)
    {
        _context = context;
    }
    
    public (Response response, string ID) Create(RepositoryCreateDto repository)
    {
        var entity = _context.Repositories.FirstOrDefault(r => r.ID == repository.Id);
        Response res;

        if (entity is null)
        {
            entity = new RepositoryEntity()
            {
                ID = repository.Id,
                LastCommitSha = repository.LastCommitSha
            };
            _context.Repositories.Add(entity);
            _context.SaveChanges();
            res = Response.Created;
        }
        else
        {
            res = Response.Conflict;
        }

        var created = new RepositoryDto(entity.ID, entity.LastCommitSha);
        return (res, created.Id);
    }
    

    public Response Update(RepositoryUpdateDto repository)
    {
        var entity = _context.Repositories.FirstOrDefault(r => r.ID == repository.Id );
        Response res;

        if (entity is not null)
        {
            entity.LastCommitSha = repository.LastCommitSha;
            _context.SaveChanges();
            res = Response.Updated;
        }else if(_context.Repositories.FirstOrDefault(r=>r.ID !=repository.Id)!=null) {
            res = Response.Conflict;
        }else {
            res = Response.NotFound;
        }
        return res;
    }

    public bool checkLatestSha(RepositoryUpdateDto repository)
    {
        var entity = _context.Repositories.FirstOrDefault(r => r.ID == repository.Id );
        Response res;

        if (entity is not null)
        {
            if (entity.LastCommitSha == repository.LastCommitSha)
            {
                return true;
            }
        }
        return false;

    }


}