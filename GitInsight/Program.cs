using CommandLine;
using GitInsight.Core;
using Microsoft.EntityFrameworkCore.Metadata;

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
        Database db = new Database();
        var repository = new Repository(options.RepositoryPath);
        db.AddRepository(repository);

        if (options.AuthorMode)
        {
            PrintAuthorModeFromDb(repository,db);
        }
        else
        {
            PrintFrequencyModeFromDb(repository, db);
        }
    }
    

    private static void PrintFrequencyModeFromDb(IRepository repo, IDatabase db)
    {
        var list = db.getCommitsPrDay(repo);
        foreach (var v in list)
        {
            Console.WriteLine(v);
        }
    }

    private static void PrintAuthorModeFromDb(IRepository repo, IDatabase db)
    {
        var authors = db.getCommitsPrAuthor(repo);
        foreach (var v in authors)
        {
            Console.WriteLine(v.author);
            foreach (var strings in v.Item2)
            {
                Console.WriteLine(strings);
            }
            
            
        }
    }
}
