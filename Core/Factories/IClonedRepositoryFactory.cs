using GitInsight.Core.Services;

namespace GitInsight.Core.Factories;

public interface IClonedRepositoryFactory
{
    IClonedRepository Create(string path);
}
