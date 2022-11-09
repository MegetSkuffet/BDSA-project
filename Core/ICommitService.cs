namespace GitInsight.Core;


public interface ICommitService
{
    (Response Response, string CommitId, DateTime date) Create(CommitDTO commit);
    IReadOnlyCollection<CommitDTO> ReadAllCommits();
    // CommitDTO Read(string CommitId);

    // TaskDetailsDTO Read(int taskId);
}