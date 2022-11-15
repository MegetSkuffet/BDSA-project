namespace GitInsight.Core;


public interface ICommitService
{
    (Response Response, string CommitId, DateTimeOffset date) Create(CommitDTO commit);
    IReadOnlyCollection<CommitDTO> GetAllCommits();
    public IEnumerable<string> getCommitsPrDay(string RID);

    public IEnumerable<(string author, IEnumerable<string>)> getCommitsPrAuthor(String RID);
}