using Newtonsoft.Json;
using PiHoleUpdater.Common.Abstractions;
using PiHoleUpdater.Common.Logging;
using PiHoleUpdater.Common.Models;
using PiHoleUpdater.Common.Models.Config;

namespace PiHoleUpdater.Common.Services;

public interface IPiHoleUpdaterService
{
  Task TickAsync(CancellationToken stoppingToken);
}

public class PiHoleUpdaterService : IPiHoleUpdaterService
{
  private readonly ILoggerAdapter<PiHoleUpdaterService> _logger;
  private readonly PiHoleUpdaterConfig _config;
  private readonly IDateTimeAbstraction _dateTime;
  private readonly UpdateServiceState _serviceState;
  private readonly IRepoManagerService _repoManager;
  private readonly IAdListService _adListService;
  private readonly string _stateFilePath;

  public PiHoleUpdaterService(ILoggerAdapter<PiHoleUpdaterService> logger,
    PiHoleUpdaterConfig config,
    IDateTimeAbstraction dateTime,
    IRepoManagerService repoManager,
    IAdListService adListService)
  {
    _logger = logger;
    _config = config;
    _dateTime = dateTime;
    _repoManager = repoManager;
    _adListService = adListService;

    _stateFilePath = GenerateStateFilePath();
    _serviceState = GetUpdaterState();
  }


  // Public methods
  public async Task TickAsync(CancellationToken stoppingToken)
  {
    if (!CanTick(stoppingToken))
      return;

    _serviceState.LastTick = DateTime.Now;

    if (_config.Repo.Enabled)
      _repoManager.UpdateLocalRepo();

    await _adListService.TickAsync(stoppingToken);

    if (_config.Repo.Enabled)
      _repoManager.CommitChanges();

    _serviceState.NextTick = new DateTime(_serviceState.LastTick.Year,
      _serviceState.LastTick.Month,
      _serviceState.LastTick.Day,
      _serviceState.LastTick.Hour,
      _serviceState.LastTick.Minute,
      0
    ).AddDays(1);

    SaveStateFile();
  }


  // Internal methods
  private string GenerateStateFilePath()
  {
    var stateFile = _config.ListGeneration.ServiceStateFile;

    if (stateFile.StartsWith("./"))
      stateFile = UpdaterUtils.ExeRelative(stateFile.Replace("./", ""));

    return stateFile;
  }

  private bool CanTick(CancellationToken stoppingToken)
  {
    if (stoppingToken.IsCancellationRequested)
      return false;

    return _dateTime.Now >= _serviceState.NextTick;
  }

  private UpdateServiceState GetUpdaterState()
  {
    if (!File.Exists(_stateFilePath))
      return new UpdateServiceState();

    var stateJson = File.ReadAllText(_stateFilePath);
    return JsonConvert.DeserializeObject<UpdateServiceState>(stateJson)!;
  }

  private void SaveStateFile()
  {
    var stateJson = JsonConvert.SerializeObject(_serviceState, Formatting.Indented);
    File.WriteAllText(_stateFilePath, stateJson);
  }
}
