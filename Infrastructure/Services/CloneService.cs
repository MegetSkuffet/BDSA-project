using GitInsight.Core.Services;
using LibGit2Sharp;

namespace Infrastructure.Services;

public class CloneService : ICloneService
{
    public Task<string> CloneRepositoryFromWebAsync(string gitUser, string gitRepository)
    {
        var repoURL = $"https://github.com/{gitUser}/{gitRepository}.git";
        
        var localDir = "./repo";

        return Task.Factory.StartNew(() => Repository.Clone(repoURL, localDir),
            TaskCreationOptions.LongRunning);
    }

    public bool FindRepositoryOnMachine(string gitRepository, out string repoPath)
    {
        var localDir = Directory.GetCurrentDirectory();

        repoPath = Path.Combine(localDir, gitRepository);

        return Directory.Exists(repoPath);
    }
}