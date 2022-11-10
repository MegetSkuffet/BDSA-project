using GitInsight.Core;

namespace GitInsight;

public interface IDatabase
{
    public void frequencyMode(IRepository repository);
    //Adds repository commit entities and repository entity to db in frequency mode. Makes sure to check if latest version is newest, reads if so, writes if not.

    public IReadOnlyCollection<CommitDTO> getAllCommits(IRepository repository);
    //Returns list of commits for the repository.
}