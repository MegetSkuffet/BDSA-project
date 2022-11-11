using GitInsight.Core;
namespace Infrastructure;

public class AuthorService : IAuthorService
{
    private readonly GitInsightContext _context;

    public AuthorService(GitInsightContext context)
    {
        _context = context;
    }

    public (Response Response, string Username) Create(AuthorCreateDTO Author)
    {
        var entity = _context.CommitsPrAuthor.FirstOrDefault(a => a.Username == Author.Username && a.date == Author.date && a.RID == Author.RID);
        Response res;
        if(entity is null)
        {
            entity = new AuthorEntity() 
            {
                Username = Author.Username,
                RID = Author.RID,
                date = Author.date,
                amountPerDay = Author.amountPerDay
            };
            _context.CommitsPrAuthor.Add(entity);
            _context.SaveChanges();
            res = Response.Created;
        } 
        else
        {
            res = Response.Conflict;
            entity.amountPerDay++;
        }
        var created = new AuthorDTO(entity.Username, entity.RID, entity.date, entity.amountPerDay);
        return (res, created.Username); 
    }

    public IReadOnlyCollection<AuthorDTO> ReadAllByAuthor()
    {
        var authors = from a in _context.CommitsPrAuthor
            orderby a.Username
            select new AuthorDTO(a.Username,a.RID,a.date,a.amountPerDay);
        
        return authors.ToList();
    }
    
    public IReadOnlyCollection<AuthorDTO> ReadAllByAuthorWithRID(string RID)
    {
        var authors = from a in _context.CommitsPrAuthor
            orderby a.Username
            where a.RID == RID
            select new AuthorDTO(a.Username,a.RID,a.date,a.amountPerDay);
        Console.WriteLine(authors.Count());

        return authors.ToList();
    }
}