using CommandLine;
using GitInsight.Core;
using Microsoft.EntityFrameworkCore.Metadata;

namespace GitInsight;

public static class Program
{
    public static void Main(string[] args)
    {
       
        var repository = new Repository(@"C:\Users\Johan\Desktop\ITU\3. semester\BDSA\assignment-02");
        Database db = new Database();
        
        
        db.AddRepository(repository);
        PrintFrequencyModeFromDb(db.getCommitsPrDay(repository));
        PrintAuthorModeFromDb(db.getCommitsPrAuthor(repository));
        db.AddRepository(repository);

        
        
        
        
    }

    private static void PrintFrequencyModeFromDb(IEnumerable<string> list)
    {
        foreach (var v in list)
        {
            Console.WriteLine(v);
        }
    }

    private static void PrintAuthorModeFromDb(IEnumerable<(string author, IEnumerable<string>)> authors)
    {
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
