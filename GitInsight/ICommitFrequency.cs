namespace GitInsight;

public interface ICommitFrequency
{
    IEnumerable<string> GetAll();

    IEnumerable<(string Author, IEnumerable<string> Lines)> GetGroupedByAuthor();
}
