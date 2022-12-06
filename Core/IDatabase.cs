using LibGit2Sharp;
using GitInsight.Core.Abstractions;

namespace GitInsight;

public interface IDatabase
{
    public void AddRepository(IInsightRepository repository);
    //Adds repository commit entities and repository entity to db in frequency mode. Makes sure to check if latest version is newest, reads if so, writes if not.

    public IEnumerable<(int count, DateTime date)> GetCommitsPrDay(IInsightRepository repository);
    //Returns list of commits for the repository.

    public IReadOnlyDictionary<string, IEnumerable<(int commitCount, DateTime date)>> GetCommitsPrAuthor(
        IInsightRepository repository);
}