namespace Infrastructure;

using Infrastructure.Core;

public class RepositoryService : IRepositoryService{
    
    private readonly GitInsightContext _context;

    public RepositoryService(GitInsightContext _context){

        _context = context;

    }

    public (Response response, string ID) create(RepositoryCreateDTO repository){

    } 

    public Response Update(RepositoryUpdateDTO repository){

    }
}