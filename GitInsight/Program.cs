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
        
        
        db.frequencyMode(repository);
        PrintFrequencyModeFromDb(db.getAllCommits());
        db.frequencyMode(repository);
        PrintFrequencyModeFromDb(db.getAllCommits());
        
        
    }

    private static void PrintFrequencyModeFromDb(IReadOnlyCollection<CommitDTO> commits)
    {
        foreach (var v in commits)
        {
            Console.WriteLine(v.date + ": " + v.amountPrDay);
        }
    }
}
