namespace Infrastructure.Core;


public interface ICommitService
{
    (Response Response, string CommitId) Create(CommitCreateDTO commit);
    IReadOnlyCollection<CommitDTO> ReadAll();
    CommitDTO Read(string CommitId);


}