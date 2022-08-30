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
  private readonly IBlockListWebProvider _blockListWebProvider;
  private readonly IBlockListEntryParser _entryParser;
  private readonly IBlockListFileWriter _blockListFileWriter;
  private readonly UpdaterConfig _config;

  public ListUpdaterService(ILoggerAdapter<ListUpdaterService> logger,
    UpdaterConfig config,
    IBlockListWebProvider blockListWebProvider,
    IBlockListEntryParser entryParser,
    IBlockListFileWriter blockListFileWriter)
  {
    _logger = logger;
    _config = config;
    _blockListWebProvider = blockListWebProvider;
    _entryParser = entryParser;
    _blockListFileWriter = blockListFileWriter;
  }

  public async Task TickAsync(CancellationToken stoppingToken)
  {
    var blockLists = new CompiledBlockLists();

    _logger.LogInformation("Processing lists...");
    foreach (var (listCategory, listEntries) in _config.BlockLists)
    {
      Console.WriteLine($"Processing list: {listCategory}");
      foreach (BlockListConfig listConfig in listEntries)
      {
        var rawBlockList = await _blockListWebProvider.GetBlockListAsync(listConfig.ListUrl);
        var addCount = blockLists.AddDomains(listCategory,
          _entryParser.ParseList(rawBlockList),
          listConfig.Restrictive);

        if (addCount > 0)
          Console.WriteLine($"  > Added {addCount} new entries");
      }
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
