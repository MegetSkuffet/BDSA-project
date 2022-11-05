using Infrastructure.Core;
namespace Infrastructure;

public class AuthorService : IAuthorService
{
    private readonly GitInsightContext _context;

    publix AuthorService(GitInsightContext context)
    {
        _context = context;
    }

    (Response Response, string Username) Create(AuthorCreateDTO Author)
    {
        var entity = _context.CommitsPrAuthor.FirstOrDefault(a => a.Username == Author.Username);
        Response res;
        if(entity is null)
        {
            entity = new AuthorDTO() 
            {
                Username = Author.Username,
                RID = Author.RID,
                date = Author.date,
                amountPerDay = Author.amountPerDay
            };
            _context.CommitsPrAuthor.Add(entity);
            _context.CommitsPrAuthor.SaveChanges();
            res = Response.Created;
        } 
        else
        {
            res = Response.Conflict;
        }
        var created = new RepositoryDTO(entity.Username, entity.RID, entity.date, entity.amountPerDay);
        return (res, created); 
    }

}