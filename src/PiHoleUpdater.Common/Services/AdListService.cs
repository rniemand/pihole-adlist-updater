using PiHoleUpdater.Common.Abstractions;
using PiHoleUpdater.Common.Enums;
using PiHoleUpdater.Common.Logging;
using PiHoleUpdater.Common.Models.Config;
using PiHoleUpdater.Common.Models.Repo;
using PiHoleUpdater.Common.Providers;
using PiHoleUpdater.Common.Repo;
using PiHoleUpdater.Common.Utils;

namespace PiHoleUpdater.Common.Services;

public interface IAdListService
{
  Task TickAsync(CancellationToken stoppingToken);
}

public class AdListService : IAdListService
{
  private readonly ILoggerAdapter<AdListService> _logger;
  private readonly IBlockListProvider _listProvider;
  private readonly IBlockListParser _listParser;
  private readonly IBlockListFileWriter _listWriter;
  private readonly IDomainTrackingService _domainTracker;
  private readonly IDateTimeAbstraction _dateTime;
  private readonly PiHoleUpdaterConfig _config;
  private DateTime _nextRunTime;

  public AdListService(ILoggerAdapter<AdListService> logger,
    IBlockListProvider listProvider,
    IBlockListParser listParser,
    IBlockListFileWriter listWriter,
    IDomainTrackingService domainTracker,
    IDateTimeAbstraction dateTime,
    PiHoleUpdaterConfig config)
  {
    _logger = logger;
    _config = config;
    _dateTime = dateTime;
    _domainTracker = domainTracker;
    _listProvider = listProvider;
    _listParser = listParser;
    _listWriter = listWriter;

    _nextRunTime = dateTime.Now.AddMinutes(-1);
  }


  // Interface methods
  public async Task TickAsync(CancellationToken stoppingToken)
  {
    if (_dateTime.Now < _nextRunTime)
      return;

    await RunListGenerationAsync();
    _nextRunTime = _dateTime.Now.AddHours(12);
    _logger.LogInformation("All done.");
  }


  // Internal methods
  private async Task RunListGenerationAsync()
  {
    _logger.LogInformation("Generating AdLists");
    foreach (var adListCategory in _config.AdListCategories.Where(x => x.Enabled))
    {
      var domains = new HashSet<BlockListEntry>();
      var listName = adListCategory.Name;

      _logger.LogInformation("Processing list: {list}", listName);
      foreach (var sourceList in adListCategory.Sources.Where(s => s.Enabled))
      {
        sourceList.List = adListCategory.Name;
        var rawListResponse = await _listProvider.GetBlockListAsync(sourceList);

        var addedCount = _listParser.AppendNewEntries(domains, listName, rawListResponse);
        if (addedCount == 0)
          continue;

        _logger.LogDebug("Added {count} new entries to list: {list}", addedCount, listName);
      }

      await _domainTracker.TrackListEntries(listName, domains);
    }

    if (_config.ListGeneration.GenerateCategoryLists)
    {
      _logger.LogInformation("Generating category lists");
      foreach (var listCategory in Enum.GetNames<AdList>())
      {
        var listType = ListQueryHelper.AdListFromString(listCategory);
        if (listType == AdList.Unknown)
          continue;

        await _listWriter.WriteCategoryLists(listType);
      }
    }

    if (_config.ListGeneration.GenerateCombinedLists)
    {
      _logger.LogInformation("Generating combined list");
      await _listWriter.WriteCombinedLists();
    }
  }
}
