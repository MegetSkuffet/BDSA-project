using GitInsight.Abstractions;
using GitInsight.Core.Services;

namespace Infrastructure.Services;

public class CommitFrequencyService : ICommitFrequencyService
{

    public IEnumerable<(int commitCount, DateTime date)> GetAll(IInsightRepository repository)
    {
        var commits = repository.Commits.ToList();
        
        return commits.GroupBy(c => c.Author.When.Date)
            .Select(g => (g.Count(),g.Key));
    }

    public IReadOnlyDictionary<string, (int commitCount, DateTime date)> GetGroupedByAuthor(IInsightRepository repository)
    {
        var commits = repository.Commits.ToList();

        var authors = commits.Select(c => c.Author.Name)
            .Distinct();
        var countsAndDates = commits
            .GroupBy(i => i.Author.When.Date)
            .Select(g => (g.Count(),g.Key));

        return authors.Zip(countsAndDates, (k, v) => new { k, v }).
            ToDictionary(l => l.k, l => l.v);
    }

    
}
