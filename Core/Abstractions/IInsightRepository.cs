namespace GitInsight.Core.Abstractions;

public interface IInsightRepository
{
    IEnumerable<InsightCommit> Commits { get; }
}
