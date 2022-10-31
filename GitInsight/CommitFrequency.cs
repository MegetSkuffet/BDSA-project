namespace GitInsight;

public class CommitFrequency : ICommitFrequency
{
    private readonly IInsightRepository _repository;

    public CommitFrequency(IInsightRepository repository)
    {
        _repository = repository;
    }

    public IEnumerable<string> GetAll()
    {
        var commits = _repository.Commits.ToList();

        return commits.GroupBy(c => c.Author.When.Date)
            .Select(g => $"{g.Count()} {g.Key:dd-MM-yy}");
    }

    public IEnumerable<(string, IEnumerable<string>)> GetGroupedByAuthor()
    {
        var commits = _repository.Commits.ToList();

        var authors = commits.Select(c => c.Author.Name)
            .Distinct();

        foreach (var author in authors)
        {
            yield return (author, commits.Where(c => c.Author.Name == author)
                .GroupBy(i => i.Author.When.Date)
                .Select(g => $"{g.Count()} {g.Key:dd-MM-yy}"));
        }
    }
}
