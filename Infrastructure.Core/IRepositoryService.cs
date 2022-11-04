namespace Infrastructure.Core;


public interface IRepositoryService
{
    (Response Response, string ID) Create(RepositoryCreateDTO repository);

}