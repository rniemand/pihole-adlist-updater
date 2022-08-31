using PiHoleUpdater.Common.Logging;
using PiHoleUpdater.Common.Models;
using PiHoleUpdater.Common.Providers;
using PiHoleUpdater.Common.Utils;

namespace PiHoleUpdater.Common.Services;

public interface IListUpdaterService
{
  Task TickAsync(CancellationToken stoppingToken);
}

public class ListUpdaterService : IListUpdaterService
{
  private readonly ILoggerAdapter<ListUpdaterService> _logger;
  private readonly IBlockListWebProvider _blockListProvider;
  private readonly IBlockListEntryParser _domainParser;
  private readonly IBlockListFileWriter _blockListFileWriter;
  private readonly IDomainTrackerService _domainTracker;
  private readonly UpdaterConfig _config;

  public ListUpdaterService(ILoggerAdapter<ListUpdaterService> logger,
    IBlockListWebProvider blockListWebProvider,
    IBlockListEntryParser domainParser,
    IBlockListFileWriter blockListFileWriter,
    IDomainTrackerService domainTracker,
    UpdaterConfig config)
  {
    _logger = logger;
    _config = config;
    _domainTracker = domainTracker;
    _blockListProvider = blockListWebProvider;
    _domainParser = domainParser;
    _blockListFileWriter = blockListFileWriter;
  }

  public async Task TickAsync(CancellationToken stoppingToken)
  {
    var blockLists = new CompiledBlockLists();

    _logger.LogInformation("Processing lists...");
    foreach (BlockListConfig blockList in _config.BlockLists.Where(x => x.Enabled))
    {
      _logger.LogInformation("Processing block list: {name}", blockList.Name);
      foreach (BlockListConfigEntry entry in blockList.Entries)
      {
        var rawList = await _blockListProvider.GetBlockListAsync(entry.Url);
        var newEntryCount = blockLists.AddDomains(blockList.Name, _domainParser.ParseList(rawList), entry.Restrictive);
        if (newEntryCount == 0)
          continue;
        _logger.LogDebug("Added {count} new entries to list: {list}", newEntryCount, blockList.Name);
      }
      await _domainTracker.TrackListEntries(blockList.Name, blockLists.GetRawEntries(blockList.Name));
    }

    if (_config.ListGeneration.GenerateCategoryLists)
    {
      _logger.LogInformation("Generating category lists...");
      foreach (var listCategory in blockLists.Categories)
        _blockListFileWriter.WriteCategoryLists(listCategory, blockLists);
    }

    if (_config.ListGeneration.GenerateCombinedLists)
    {
      _logger.LogInformation("Generating combined lists...");
      _blockListFileWriter.WriteCombinedLists(blockLists);
    }

    _logger.LogInformation("All done.");
  }
}
