namespace Infrastructure;

using Infrastructure.Core;

public class RepositoryService : IRepositoryService
{

    private readonly GitInsightContext _context;

    public RepositoryService(GitInsightContext _context)
    {

        _context = context;

    }

    public (Response response, string ID) create(RepositoryCreateDTO repository)
    {
        var entity = _context.Repositories.FirstOrDefault(r => r.ID == repository.ID);

        Response res;

        if (entity is null)
        {
            entity = new Repository()
            {
                ID = repository.ID,
                LastcommitSha = repository.LastcommitSha
            };
            _context.Repositories.Add(entity);
            _context.Repositories.SaveChanges();
            res = Response.Created;
        }
        else
        {
            res = Response.Conflict;
        }

        var created = new RepositoryDTO(entity.ID, entity.LastcommitSha);
        return (res, created);
    }

    public Response Update(RepositoryUpdateDTO repository)
    {
        var entity = _context.Repositories.FirstOrDefault(r => r.ID == repository.ID);

        Response res;

        if (entity is not null)
        {

            entity.LastcommitSha = repository.LastcommitSha;
            _context.Repositories.SaveChanges();
            res = Response.Updated;
        }
        else
        {
            res = Response.NotFound;
        }
        return res;
    }
}