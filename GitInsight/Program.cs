using CommandLine;
using GitInsight.Core;
using Microsoft.EntityFrameworkCore.Metadata;

namespace GitInsight;

public static class Program
{
    public static void Main(string[] args)
    {
       
        var repository = new Repository(@"C:\Users\johan\OneDrive\Desktop\3.Semester\BDSA\Assignment4_bdsa\Assignment_04");
        Database db = new Database();
        
        
        Console.WriteLine("First repository:");
        db.AddRepoEntities(repository);
        //PrintFrequencyModeFromDb(db.getAllCommits(repository));
        PrintAuthorModeFromDb(db.getAllAuthors(repository));
        
        /*Console.WriteLine("Second repository:");
        repository = new Repository(@"C:\Users\johan\OneDrive\Desktop\3.Semester\BDSA\Assignment4_bdsa\Assignment_04");
        db.frequencyMode(repository);
        PrintFrequencyModeFromDb(db.getAllCommits(repository));
        
        Console.WriteLine("First again:");
         repository = new Repository(@"C:\Users\johan\OneDrive\Desktop\3.Semester\BDSA\assignment3_bdsa\Assignment_03");
         db.frequencyMode(repository);
         PrintFrequencyModeFromDb(db.getAllCommits(repository));*/
         
        
        
        
    }

    private static void PrintFrequencyModeFromDb(IReadOnlyCollection<CommitDTO> commits)
    {
        foreach (var v in commits)
        {
            Console.WriteLine(v.date + ": " + v.amountPrDay);
        }
    }

    private static void PrintAuthorModeFromDb(IReadOnlyCollection<AuthorDTO> commits)
    {
        foreach (var v in commits)
        {
            Console.WriteLine(v.Username + ":");
            Console.WriteLine(v.date + ": " + v.amountPerDay);
            
        }
    }
}
