namespace GitInsight;

public interface ICommitFrequency
{
    IEnumerable<string> GetAll();

    IEnumerable<string> GetGroupedByAuthor();
}
