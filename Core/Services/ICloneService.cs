using System.Diagnostics.CodeAnalysis;

namespace GitInsight.Core.Services;

public interface ICloneService
{
    Task<IClonedRepository> CloneRepositoryFromWebAsync(string user, string repository);

    bool FindRepositoryOnMachine(string user, string repository, [NotNullWhen(true)] out IClonedRepository? handle);
}
