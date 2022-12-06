using GitInsight.Core.Factories;
using GitInsight.Core.Services;

namespace GitInsight.Factories;

public class ClonedRepositoryFactory : IClonedRepositoryFactory
{
    private readonly IServiceProvider _serviceProvider;

    public ClonedRepositoryFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IClonedRepository Create(string path)
    {
        var handle = _serviceProvider.GetRequiredService<ClonedRepository>();
        handle.Path = path;

        return handle;
    }

    public class ClonedRepository : IClonedRepository
    {
        public string Path { get; set; } = null!;

    }
}
