using AutoFixture;
using AutoFixture.AutoNSubstitute;
using EntityFrameworkCore.AutoFixture.Sqlite;

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
        fixture.Customize(new SqliteCustomization());

        return fixture;
    }
}
