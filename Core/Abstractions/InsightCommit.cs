using LibGit2Sharp;

namespace GitInsight.Core.Abstractions;

public record InsightCommit(InsightSignature Author)
{
    public static InsightCommit FromCommit(Commit commit)
    {
        var insightAuthor = InsightSignature.FromSignature(commit.Author);

        return new InsightCommit(insightAuthor);
    }
}
