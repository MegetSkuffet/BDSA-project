namespace Infrastructure;

using Microsoft.Extensions.Hosting;

public class ApplicationLifetimeService : IHostedService
{

    private readonly IHostApplicationLifetime appLifetime;
    public ApplicationLifetimeService(IHostApplicationLifetime appLifetime)
    {
        this.appLifetime = appLifetime;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        appLifetime.ApplicationStarted.Register(OnStarted);
        appLifetime.ApplicationStopped.Register(OnStopped);
        appLifetime.ApplicationStopping.Register(OnStopping);

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    private void OnStarted()
    {

    }

    private void OnStopped()
    {

    }

    private void OnStopping()
    {
        System.IO.DirectoryInfo di = new DirectoryInfo("../GitInsight/repo");
        setAttributesNormal(di);
        
        foreach (FileInfo file in di.GetFiles())
        {
            file.Delete();
        }
        foreach (DirectoryInfo dir in di.GetDirectories())
        {
            dir.Delete(true);
        }
    }

    private void setAttributesNormal(DirectoryInfo dir)
    {
        foreach (var file in dir.GetFiles())
        {
            file.Attributes = FileAttributes.Normal;
            if (file.IsReadOnly)
            {
                file.IsReadOnly = false;
            }
        }
        foreach (var subDir in dir.GetDirectories())
        {
            subDir.Attributes = FileAttributes.Normal;
            setAttributesNormal(subDir);
        }
    }
}