namespace Infrastructure.Core;


public interface IRepositoryService
{
    (Response response, string ID, string LastcommitSha) Create(RepositoryCreateDTO repository);

    Response Update(RepositoryUpdateDTO repository);
}