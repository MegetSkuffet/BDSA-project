using Humanizer;

namespace GitInsight;

public class CommitFrequency : ICommitFrequencyService
{
    private readonly IInsightRepository _repository;

    public CommitFrequency(IInsightRepository repository)
    {
        _repository = repository;
    }

    IEnumerable<(int commitCount, DateTime date)> ICommitFrequencyService.GetAll()
    {
        var commits = _repository.Commits.ToList();
        
        return commits.GroupBy(c => c.Author.When.Date)
            .Select(g => (g.Count(),g.Key));
    }

    public IReadOnlyDictionary<string, (int commitCount, DateTime date)> GetGroupedByAuthor()
    {
        var commits = _repository.Commits.ToList();

        var authors = commits.Select(c => c.Author.Name)
            .Distinct();
        var countsAndDates = commits
            .GroupBy(i => i.Author.When.Date)
            .Select(g => (g.Count(),g.Key));

        return authors.Zip(countsAndDates, (k, v) => new { k, v }).
            ToDictionary(l => l.k, l => l.v);
    }

    
}
