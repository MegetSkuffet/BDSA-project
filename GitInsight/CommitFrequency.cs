namespace GitInsight;

public class CommitFrequency
{

    private readonly IRepository _repository;

    public CommitFrequency(IRepository repository)
    {
        _repository = repository;
    }
}