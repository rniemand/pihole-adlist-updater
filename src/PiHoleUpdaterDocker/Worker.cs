using PiHoleUpdater.Common.Logging;
using PiHoleUpdater.Common.Services;

namespace PiHoleUpdaterDocker
{
  public class Worker : BackgroundService
  {
    private readonly IPiHoleUpdaterService _updaterService;
    private readonly ILoggerAdapter<Worker> _logger;

    public Worker(IPiHoleUpdaterService updaterService, ILoggerAdapter<Worker> logger)
    {
      _updaterService = updaterService;
      _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
      try
      {
        while (!stoppingToken.IsCancellationRequested)
        {
          await _updaterService.TickAsync(stoppingToken);
          await Task.Delay(1000, stoppingToken);
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex);
        _logger.LogError(ex, "Failed to run: {error}", ex.Message);
        throw;
      }
    }
  }
}
