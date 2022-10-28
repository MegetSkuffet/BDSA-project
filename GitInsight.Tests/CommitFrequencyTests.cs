namespace GitInsight.Tests;

public class CommitFrequencyTests
{
    [Theory]
    [GitInsightData]
    public void GetAll_ReturnsFrequenciesAndDates_GivenCommits([Frozen] IRepository repository,
        CommitFrequency frequency)
    {
    }
}
