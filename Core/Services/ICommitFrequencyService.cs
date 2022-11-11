namespace GitInsight;

public interface ICommitFrequencyService
{
    IEnumerable<(int commitCount,DateTime date)> GetAll();

    IReadOnlyDictionary<string,(int commitCount, DateTime date)> GetGroupedByAuthor();
}
