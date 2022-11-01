using AutoFixture;
using AutoFixture.AutoNSubstitute;

namespace GitInsight.UnitTests;

public class GitInsightDataAttribute : AutoDataAttribute
{
    public GitInsightDataAttribute() : base(CreateFixture)
    {
    }

    private static IFixture CreateFixture()
    {
        var fixture = new Fixture();
        fixture.Customize(new AutoNSubstituteCustomization { ConfigureMembers = true });

        return fixture;
    }
}
