using PiHoleUpdater.Common.Services;

namespace PiHoleUpdaterDocker
{
  public class Worker : BackgroundService
  {
    private readonly IPiHoleUpdaterService _updaterService;

    public Worker(IPiHoleUpdaterService updaterService)
    {
      _updaterService = updaterService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
      while (!stoppingToken.IsCancellationRequested)
      {
        await _updaterService.TickAsync(stoppingToken);
        await Task.Delay(1000, stoppingToken);
      }
    }
  }
}
