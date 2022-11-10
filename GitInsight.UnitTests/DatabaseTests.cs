using LibGit2Sharp;

namespace GitInsight.UnitTests;

public class DatabaseTests
{
        private Database _db;
        private Repository _repository;
        
    public DatabaseTests()
    {
        _db = new Database();
         _repository = new Repository(@"C:\Users\Johan\Desktop\ITU\3. semester\BDSA\assignment-02");

    }

    [Fact]
    public void read_when_empty_returns_empty()
    {
        var actual =_db.getAllCommits(_repository);
        actual.Should().BeEmpty();
        
    }

    [Fact]
    public void frequencyMode_should_fill_commitDbSet()
    {
        _db.frequencyMode(_repository);
        var actual = _db.getAllCommits(_repository);
        actual.Count.Should().BeGreaterThan(3);
    }

    [Fact]
    public void frequencyMode_should_put_1_in_repoDbSet()
    {
        _db.frequencyMode(_repository);
        var actual = _db.Context.Repositories.Count();
        actual.Should().Be(1);
    }
    
}