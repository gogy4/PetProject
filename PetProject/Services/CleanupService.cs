using PetProject.Data;

namespace PetProject.Services;

public class CleanupService : IHostedService, IDisposable
{
    private readonly IServiceScopeFactory scopeFactory;
    private Timer timer;

    public CleanupService(IServiceScopeFactory scopeFactory)
    {
        this.scopeFactory = scopeFactory;
    }

    public void Dispose()
    {
        timer?.Dispose();
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        var currentTime = DateTime.Now;
        var nextRunTime = currentTime.Date.AddDays(1).AddHours(2);
        var initialDelay = nextRunTime - currentTime;

        timer = new Timer(ExecuteTask, null, initialDelay, TimeSpan.FromDays(7));

        return Task.CompletedTask;
    }

    private async void ExecuteTask(object state)
    {
        using var scope = scopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        await dbContext.RemoveExpiredPastesAsync();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        timer?.Change(Timeout.Infinite, 0);
        return Task.CompletedTask;
    }
}