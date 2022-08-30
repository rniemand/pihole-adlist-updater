using PiHoleUpdater.Common.Logging;
using PiHoleUpdater.Common.Models;
using PiHoleUpdater.Common.Providers;

namespace PiHoleUpdater.Common.Services;

public interface IListUpdaterService
{
  Task TickAsync(CancellationToken stoppingToken);
}

public class ListUpdaterService : IListUpdaterService
{
  private readonly ILoggerAdapter<ListUpdaterService> _logger;
  private readonly IBlockListWebProvider _blockListWebProvider;
  private readonly UpdaterConfig _config;

  public ListUpdaterService(ILoggerAdapter<ListUpdaterService> logger,
    UpdaterConfig config,
    IBlockListWebProvider blockListWebProvider)
  {
    _logger = logger;
    _config = config;
    _blockListWebProvider = blockListWebProvider;
  }

  public async Task TickAsync(CancellationToken stoppingToken)
  {
    var listParser = new BlockListEntryParser(_config);
    var listDumper = new BlockListFileWriter(_config);
    var blockLists = new CompiledBlockLists();

    UpdaterUtils.WriteHeading("Processing lists...");
    foreach (var (listCategory, listEntries) in _config.BlockLists)
    {
      Console.WriteLine($"Processing list: {listCategory}");
      foreach (BlockListConfig listConfig in listEntries)
      {
        var rawBlockList = await _blockListWebProvider.GetBlockListAsync(listConfig.ListUrl);
        var addCount = blockLists.AddDomains(listCategory,
          listParser.ParseList(rawBlockList),
          listConfig.Restrictive);

        if (addCount > 0)
          Console.WriteLine($"  > Added {addCount} new entries");
      }
    }

    if (_config.ListGeneration.GenerateCategoryLists)
    {
      UpdaterUtils.WriteHeading("Generating category lists...");
      foreach (var listCategory in blockLists.Categories)
        listDumper.WriteCategoryLists(listCategory, blockLists);
    }

    if (_config.ListGeneration.GenerateCombinedLists)
    {
      UpdaterUtils.WriteHeading("Generating combined lists...");
      listDumper.WriteCombinedLists(blockLists);
    }

    UpdaterUtils.WriteHeading("All done.");
  }
}
