namespace GitInsight.Core;


public interface IRepositoryService
{
    (Response response, string ID) Create(RepositoryCreateDTO repository);

    Response Update(RepositoryUpdateDTO repository);

    public bool checkLatestSha(RepositoryUpdateDTO repository);

}