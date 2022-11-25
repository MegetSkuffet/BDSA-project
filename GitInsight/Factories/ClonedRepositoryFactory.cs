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

    public class ClonedRepository : IClonedRepository, IDisposable
    {
        public string Path { get; set; } = null!;

        public void Dispose()
        {
            // Multiple instances of 'ClonedRepository' might exist for the same path.
            // Therefore, we check if the folder still exists.
            // Additionally, we ignore 'IOException', because we don't know if Dispose is accessed in parallel.
            try
            {
                if (Directory.Exists(Path))
                {
                    Directory.Delete(Path, true);
                }
            }
            catch (IOException)
            {
            }
        }
    }
}
