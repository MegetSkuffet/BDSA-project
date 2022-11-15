
using GitInsight.Core.Services;
using Infrastructure;

namespace GitInsight.UnitTests;

public class CommitFrequencyTests
{
    [Theory]
    [GitInsightData]
    public void GetAll_ReturnsFrequenciesAndDates_GivenUniqueAuthorCommits([Frozen] IInsightRepository repository,
        CommitFrequency frequency)
    {
        var result = ((ICommitFrequencyService)frequency).GetAll();

        result.Should()
            .ContainInOrder(repository.Commits.Select(c => $"{1,7} {c.Author.When:dd-MM-yy}"));
    }
}
