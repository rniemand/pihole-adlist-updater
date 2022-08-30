using PiHoleUpdater.Common.Logging;
using PiHoleUpdater.Common.Models;

namespace PiHoleUpdater.Common.Services;

public interface IListUpdaterService
{
  Task TickAsync(CancellationToken stoppingToken);
}

public class ListUpdaterService : IListUpdaterService
{
  private readonly ILoggerAdapter<ListUpdaterService> _logger;
  private readonly UpdaterConfig _config;

  public ListUpdaterService(ILoggerAdapter<ListUpdaterService> logger,
    UpdaterConfig config)
  {
    _logger = logger;
    _config = config;
  }

  public async Task TickAsync(CancellationToken stoppingToken)
  {
    var webService = new BlockListWebService(_config);
    var listParser = new BlockListEntryParser(_config);
    var listDumper = new BlockListFileWriter(_config);
    var blockLists = new CompiledBlockLists();

    UpdaterUtils.WriteHeading("Processing lists...");
    foreach (var (listCategory, listEntries) in _config.BlockLists)
    {
      Console.WriteLine($"Processing list: {listCategory}");
      foreach (var listConfig in listEntries)
      {
        var rawBlockList = await webService.GetBlockListAsync(listConfig.ListUrl);
        var addCount = blockLists.AddDomains(listCategory,
          listParser.ParseList(rawBlockList),
          listConfig.Restrictive);

        if (addCount > 0)
          Console.WriteLine($"  > Added {addCount} new entries");
      }
    }

    if (config.ListGeneration.GenerateCategoryLists)
    {
      UpdaterUtils.WriteHeading("Generating category lists...");
      foreach (var listCategory in blockLists.Categories)
        listDumper.WriteCategoryLists(listCategory, blockLists);
    }

    if (config.ListGeneration.GenerateCombinedLists)
    {
      UpdaterUtils.WriteHeading("Generating combined lists...");
      listDumper.WriteCombinedLists(blockLists);
    }

    UpdaterUtils.WriteHeading("All done.");
  }
}
