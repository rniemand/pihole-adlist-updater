using PiHoleUpdater.Common.Abstractions;
using PiHoleUpdater.Common.Enums;
using PiHoleUpdater.Common.Logging;
using PiHoleUpdater.Common.Models;
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
  private readonly IDomainService _domainTracker;
  private readonly IDateTimeAbstraction _dateTime;
  private readonly PiHoleUpdaterConfig _config;
  private readonly IAdListRepo _listRepo;
  private DateTime _nextRunTime;

  public AdListService(ILoggerAdapter<AdListService> logger,
    IBlockListProvider listProvider,
    IBlockListParser listParser,
    IBlockListFileWriter listWriter,
    IDomainService domainTracker,
    IDateTimeAbstraction dateTime,
    PiHoleUpdaterConfig config,
    IAdListRepo listRepo)
  {
    _logger = logger;
    _config = config;
    _listRepo = listRepo;
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
    foreach (AdListCategoryConfig category in _config.AdListCategories.Where(x => x.Enabled))
    {
      var domains = new HashSet<BlockListEntry>();
      var adListSources = await GetSourceEntries(category.AdListType);

      _logger.LogInformation("Processing list: {list}", category.AdListType);
      foreach (AdListSourceEntry sourceList in adListSources)
      {
        var rawListResponse = await _listProvider.GetBlockListAsync(sourceList);

        var addedCount = await _listParser.AppendNewEntries(domains, category.AdListType, rawListResponse);
        if (addedCount == 0)
          continue;

        Console.Write($"\r > Added {addedCount} new entries to {category.AdListType}          ");
      }
      Console.WriteLine();

      await _domainTracker.TrackEntriesAsync(category.AdListType, domains);
      await _listWriter.WriteCategoryLists(category.AdListType, adListSources);
    }

    if (_config.ListGeneration.GenerateCombinedLists)
    {
      _logger.LogInformation("Generating combined list");
      await _listWriter.WriteCombinedLists();
    }
  }

  private async Task<List<AdListSourceEntry>> GetSourceEntries(AdListType listType)
  {
    var dbEntries = await _listRepo.GetSourceEntries(listType);

    return dbEntries.Select(entry => new AdListSourceEntry
    {
      Enabled = entry.Enabled,
      Maintainer = entry.Maintainer,
      ProjectUrl = entry.ProjectUrl,
      SourceType = entry.AdListType,
      ListUrl = entry.ListUrl
    })
      .ToList();
  }
}
