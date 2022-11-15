using GitInsight.Abstractions;

namespace GitInsight.Core.Services;

public interface ICommitFrequencyService
{
    IEnumerable<(int commitCount,DateTime date)> GetAll(IInsightRepository repository);

    IReadOnlyDictionary<string,(int commitCount, DateTime date)> GetGroupedByAuthor(IInsightRepository repository);
}
