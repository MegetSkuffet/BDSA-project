using LibGit2Sharp;

namespace GitInsight.UnitTests;

public class DatabaseTests
{
        private Database _db;
        private Repository _repository;
        
    public DatabaseTests()
    {
        _db = new Database(true);
         _repository = new Repository(@"C:\Users\johan\OneDrive\Desktop\Skole\3.Semester\BDSA\Assignment1_bdsa\bdsa-Assignment_01");

    }

    [Fact]
    public void frequencyMode_should_fill_commitDbSet()
    {
        _db.AddRepository(_repository);
        var actual = _db.getCommitsPrDay(_repository);
        actual.Count().Should().BeGreaterThan(1);
    }

    [Fact]
    public void frequencyMode_should_put_1_in_repoDbSet()
    {
        _db.AddRepository(_repository);
        var actual = _db.Context.Repositories.Count();
        actual.Should().Be(1);
    }

    [Fact]
    public void getCommitsPrAuthor_should_return_()
    {
        _db.AddRepository(_repository);
        var commits = _db.getCommitsPrAuthor(_repository).ToList();
        commits.Count.Should().BeGreaterThan(1);
    }
    
}