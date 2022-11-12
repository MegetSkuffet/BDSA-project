namespace GitInsight.Core;


public interface ICommitService
{
    (Response Response, string CommitId, DateTimeOffset date) Create(CommitDTO commit);
    IReadOnlyCollection<CommitDTO> GetAllCommits();
    // CommitDTO Read(string CommitId);

    // TaskDetailsDTO Read(int taskId);
}