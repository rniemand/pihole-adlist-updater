using PiHoleUpdater.Common.Abstractions;
using PiHoleUpdater.Common.Enums;
using PiHoleUpdater.Common.Logging;
using PiHoleUpdater.Common.Models.Config;
using PiHoleUpdater.Common.Models.Repo;
using PiHoleUpdater.Common.Providers;
using PiHoleUpdater.Common.Repo;
using PiHoleUpdater.Common.Utils;

namespace PiHoleUpdater.Common.Services;

public interface IListUpdaterService
{
  Task TickAsync(CancellationToken stoppingToken);
}

public class ListUpdaterService : IListUpdaterService
{
  private readonly ILoggerAdapter<ListUpdaterService> _logger;
  private readonly IBlockListProvider _listProvider;
  private readonly IBlockListParser _listParser;
  private readonly IBlockListFileWriter _listWriter;
  private readonly IDomainTrackingService _domainTracker;
  private readonly IDateTimeAbstraction _dateTime;
  private readonly PiHoleUpdaterConfig _config;
  private DateTime _nextRunTime;

  public ListUpdaterService(ILoggerAdapter<ListUpdaterService> logger,
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
    _logger.LogInformation("Processing lists...");
    foreach (BlockListCategoryConfig blockList in _config.BlockLists.Where(x => x.Enabled))
    {
      var entries = new HashSet<BlockListEntry>();
      var adList = blockList.Name;

      _logger.LogInformation("Processing block list: {name}", adList);
      foreach (BlockListConfigEntry entry in blockList.Entries)
      {
        var rawList = await _listProvider.GetBlockListAsync(entry.Url);
        var newEntryCount = _listParser.AppendNewEntries(entries, adList, rawList);

        if (newEntryCount == 0)
          continue;

        _logger.LogDebug("Added {count} new entries to list: {list}", newEntryCount, adList);
      }

      await _domainTracker.TrackListEntries(adList, entries);
    }

    if (_config.ListGeneration.GenerateCategoryLists)
    {
      _logger.LogInformation("Generating category lists...");
      foreach (var listCategory in Enum.GetNames<AdList>())
        await _listWriter.WriteCategoryLists(ListQueryHelper.AdListFromString(listCategory));
    }

    if (_config.ListGeneration.GenerateCombinedLists)
    {
      _logger.LogInformation("Generating combined lists...");
      await _listWriter.WriteCombinedLists();
    }
  }
}
