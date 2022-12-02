namespace GitInsight.UnitTests;

public class DatabaseTests
{
        private Database _db;
        //private IInsightRepository _repository;
        
    public DatabaseTests()
    {
        //_db = new Database(true);
        //_repository = Substitute.For<IInsightRepository>();

    }

    [Theory]
    [GitInsightData]
    public void frequencyMode_should_fill_commitDbSet(Database database, IInsightRepository repository)
    {
        database.AddRepository(repository);
        var actual = database.GetCommitsPrDay(repository);
        actual.Count().Should().BeGreaterThan(1);
    }

    [Theory]
    [GitInsightData]
    public void frequencyMode_should_put_1_in_repoDbSet(Database database, IInsightRepository repository)
    {
        database.AddRepository(repository);
        var actual = database.Context.Repositories.Count();
        actual.Should().Be(1);
    }

    [Theory]
    [GitInsightData]
    public void getCommitsPrAuthor_should_return_(Database database, IInsightRepository repository)
    {
        database.AddRepository(repository);
        var commits = database.GetCommitsPrAuthor(repository).ToList();
        commits.Count.Should().BeGreaterThan(1);
    }
    
}