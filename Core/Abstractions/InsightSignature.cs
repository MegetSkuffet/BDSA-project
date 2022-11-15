using LibGit2Sharp;

namespace GitInsight.Core.Abstractions;

public record InsightSignature(string Name, string Email, DateTimeOffset When)
{
    public static InsightSignature FromSignature(Signature signature)
    {
        return new InsightSignature(signature.Name, signature.Email, signature.When);
    }
}
