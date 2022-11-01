using CommandLine;

namespace GitInsight;

public static class Program
{
    public static void Main(string[] args)
    {
        var parser = new Parser(s =>
        {
            s.EnableDashDash = true;
            s.HelpWriter = Console.Error;
        });
        var result = parser.ParseArguments<Options>(args);
        var options = result.Value;

        if (result.Tag == ParserResultType.NotParsed)
        {
            return;
        }

        var repository = new Repository(options.RepositoryPath);
        var insightRepository = new InsightRepository(repository);
        var frequency = new CommitFrequency(insightRepository);

        if (options.AuthorMode)
        {
            PrintAuthorMode(frequency);
        }
        else
        {
            PrintFrequencyMode(frequency);
        }
    }

    private static void PrintFrequencyMode(ICommitFrequency frequency)
    {
        var lines = frequency.GetAll();

        foreach (var line in lines)
        {
            Console.WriteLine(line);
        }
    }

    private static void PrintAuthorMode(ICommitFrequency frequency)
    {
        var groupedLines = frequency.GetGroupedByAuthor();

        foreach (var grouping in groupedLines)
        {
            Console.WriteLine(grouping.Author);

            foreach (var line in grouping.Lines)
            {
                Console.WriteLine(line);
            }

            Console.WriteLine();
        }
    }
}
