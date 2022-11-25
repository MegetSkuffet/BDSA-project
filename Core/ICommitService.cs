namespace GitInsight.Core;


public interface ICommitService
{
    (Response Response, string CommitId, DateTimeOffset date) Create(CommitDTO commit);
    IReadOnlyCollection<CommitDTO> GetAllCommits();
    public IEnumerable<(int commitCount, DateTime date)> GetCommitsPrDay(string RID);

    public IReadOnlyDictionary<string, IEnumerable<(int commitCount, DateTime date)>> GetCommitsPrAuthor(String RID);
}