using LibGit2Sharp;

namespace GitInsight.Core.Abstractions;

public record InsightCommit(InsightSignature Author, string Sha)
{
    public static InsightCommit FromCommit(Commit commit)
    {
        var insightAuthor = InsightSignature.FromSignature(commit.Author);
        var sha = commit.Sha;

        return new InsightCommit(insightAuthor,sha);
    }
}
