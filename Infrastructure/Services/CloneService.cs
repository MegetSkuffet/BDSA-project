using System.Diagnostics.CodeAnalysis;
using GitInsight.Core.Factories;
using GitInsight.Core.Services;
using LibGit2Sharp;

namespace Infrastructure.Services;

public class CloneService : ICloneService
{
    private readonly IClonedRepositoryFactory _clonedRepositoryFactory;

    public CloneService(IClonedRepositoryFactory clonedRepositoryFactory)
    {
        _clonedRepositoryFactory = clonedRepositoryFactory;
    }

    public async Task<IClonedRepository> CloneRepositoryFromWebAsync(string repository, string user)
    {
        var repoUrl = $"https://github.com/{user}/{repository}.git";
        var tempPath = Path.GetTempPath();
        var repoPath = Path.Combine(tempPath, user, repository);

        var path = await Task.Factory.StartNew(() => Repository.Clone(repoUrl, repoPath),
            TaskCreationOptions.LongRunning);

        return _clonedRepositoryFactory.Create(path);
    }

    public bool FindRepositoryOnMachine(string user, string repository, [NotNullWhen(true)] out IClonedRepository? handle)
    {
        var tempPath = Path.GetTempPath();
        var repoPath = Path.Combine(tempPath, user, repository);
        var exists = Directory.Exists(repoPath);
        handle = exists ? _clonedRepositoryFactory.Create(repoPath) : null;

        return exists;
    }
}
