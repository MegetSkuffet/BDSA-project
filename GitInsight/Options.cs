using CommandLine;

namespace GitInsight;

public class Options
{
    public Options(string repositoryPath, bool authorMode)
    {
        RepositoryPath = repositoryPath;
        AuthorMode = authorMode;
    }

    [Value(0, Required = true)]
    public string RepositoryPath { get; }

    [Option('a', "author")]
    public bool AuthorMode { get; }
}
