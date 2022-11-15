using GitInsight.Core;
using Microsoft.EntityFrameworkCore.Metadata;

namespace GitInsight;

public static class Program
{
    public static void Main(string[] args)
    {
        Database db = new Database();
        var repository = new Repository(options.RepositoryPath);
        db.AddRepository(repository);

        CreateHostBuilder(args)
            .Build()
            .Run();
    }

    private static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>());

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
