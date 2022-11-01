namespace GitInsight.Abstractions;

public interface IInsightRepository
{
    IEnumerable<InsightCommit> Commits { get; }
}
