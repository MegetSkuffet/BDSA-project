using System.Collections.ObjectModel;
using LibGit2Sharp;

namespace GitInsight.Core.Abstractions;

public class InsightRepository : IInsightRepository, IDisposable
{
    private readonly IRepository _repository;

    private readonly List<InsightCommit> _commits;
    private readonly ReadOnlyCollection<InsightCommit> _readOnlyCommits;

    private bool _commitsInvalidated;

    public InsightRepository(IRepository repository)
    {
        _repository = repository;

        _commits = new List<InsightCommit>();
        _readOnlyCommits = new ReadOnlyCollection<InsightCommit>(_commits);

        _commitsInvalidated = true;
    }

    public IEnumerable<InsightCommit> Commits
    {
        get
        {
            if (!_commitsInvalidated)
            {
                return _readOnlyCommits;
            }

            RebuildCommits();
            _commitsInvalidated = false;

            return _readOnlyCommits;
        }
    }

    private void RebuildCommits()
    {
        _commits.Clear();
        var newCommits = _repository.Commits.Select(InsightCommit.FromCommit);
        _commits.AddRange(newCommits);
    }

    public void Dispose()
    {
        _repository.Dispose();
    }
}
