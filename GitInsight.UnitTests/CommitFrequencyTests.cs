namespace GitInsight.UnitTests;

public class CommitFrequencyTests
{
    [Theory]
    [GitInsightData]
    public void GetAll_ReturnsFrequenciesAndDates_GivenUniqueAuthorCommits([Frozen] IInsightRepository repository,
        CommitFrequency frequency)
    {
        var result = frequency.GetAll();

        result.Should()
            .ContainInOrder(repository.Commits.Select(c => $"{1,7} {c.Author.When:dd-MM-yy}"));
    }

    [Theory]
    [GitInsightData]
    public void GetAll_ReturnsFrequenciesAndDates_GivenSameAuthorCommits([Frozen] InsightCommit commit,
        [Frozen] IInsightRepository repository, CommitFrequency frequency)
    {
        var result = frequency.GetAll();

        result.Should()
            .OnlyContain(s => s == $"{repository.Commits.Count(),7} {commit.Author.When:dd-MM-yy}");
    }

    [Theory]
    [GitInsightData]
    public void GetAll_ReturnsFrequenciesAndDates_GivenMultipleSameAuthorCommits(
        [Frozen] List<InsightCommit> firstCommits, List<InsightCommit> secondCommits,
        [Frozen] IInsightRepository repository, CommitFrequency frequency)
    {
        repository.Commits.Returns(firstCommits.Concat(secondCommits));

        var result = frequency.GetAll();

        result.Should()
            .ContainInOrder(repository.Commits.Select(c => $"{2,7} {c.Author.When:dd-MM-yy}")
                .Distinct());
    }

    [Theory]
    [GitInsightData]
    public void GetAll_ReturnsEmpty_GivenNoCommits([Frozen] IInsightRepository repository, CommitFrequency frequency)
    {
        repository.Commits.Returns(Enumerable.Empty<InsightCommit>());

        var result = frequency.GetAll();

        result.Should()
            .BeEmpty();
    }
}
