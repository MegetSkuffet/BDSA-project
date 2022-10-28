namespace GitInsight;

public class CommitFrequency : ICommitFrequency
{

    private readonly IRepository _repository;

    public CommitFrequency(IRepository repository)
    {
        _repository = repository;
    }

    public IEnumerable<string> GetAll()
    {
        var commitList = _repository.Commits.ToList();

        return from g in (commitList.GroupBy(i => i.Author.When.Date))
               select String.Format("{0} {1:dd-MM-yy}", g.Count(), g.Key);

    }

    public IEnumerable<(string, IEnumerable<string>)> GetGroupedByAuthor()
    {
        var commitList = _repository.Commits.ToList();

        var authors = commitList.Select(c => c.Author.Name).Distinct();

        foreach (var a in authors)
        {
            yield return (a, commitList.Where(c => c.Author.Name == a).GroupBy(i => i.Author.When.Date).Select(g => String.Format("{0} {1:dd-MM-yy}", g.Count(), g.Key)));
        }
    }
}