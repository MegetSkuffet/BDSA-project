namespace Infrastructure.Core;


public interface ICommitService
{
    (Response Response, string CommitId) Create(CommitCreateDTO commit);
    IReadOnlyCollection<CommitDTO> ReadAllCommits();
    // CommitDTO Read(string CommitId);

    IReadOnlyCollection<CommitDTO> ReadAllByAuthor(string Author);
    // TaskDetailsDTO Read(int taskId);
}