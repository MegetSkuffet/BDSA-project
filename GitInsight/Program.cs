using LibGit2Sharp;

var repo = new Repository(@"C:\Users\malte\Documents\Exercises\c#\bdsa-assignment-03");
var commitliste = repo.Commits.ToList();


var groupItems = commitliste.GroupBy(i => i.Author.When.Date);

foreach(var group in groupItems){
    Console.WriteLine("{0} {1:dd-MM-yy}", group.Count(), group.Key);
}
