namespace GitInsight.Core;


public interface IRepositoryService
{
    (Response response, string ID) Create(RepositoryCreateDto repository);

    Response Update(RepositoryUpdateDto repository);

    public bool CheckLatestSha(RepositoryUpdateDto repository);

}