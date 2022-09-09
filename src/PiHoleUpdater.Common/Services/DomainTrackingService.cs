using PiHoleUpdater.Common.Enums;
using PiHoleUpdater.Common.Logging;
using PiHoleUpdater.Common.Models.Config;
using PiHoleUpdater.Common.Models.Repo;
using PiHoleUpdater.Common.Repo;

namespace PiHoleUpdater.Common.Services;

public interface IDomainTrackingService
{
  Task TrackListEntries(AdList list, HashSet<BlockListEntry> listEntries);
}

public class DomainTrackingService : IDomainTrackingService
{
  private readonly ILoggerAdapter<DomainTrackingService> _logger;
  private readonly IDomainRepo _domainRepo;
  private readonly int _insertBatchSize;
  private readonly int _updateBatchSize;

  public DomainTrackingService(ILoggerAdapter<DomainTrackingService> logger,
    IDomainRepo domainRepo,
    PiHoleUpdaterConfig config)
  {
    _logger = logger;
    _domainRepo = domainRepo;
    _insertBatchSize = config.ListGeneration.InsertBatchSize;
    _updateBatchSize = config.ListGeneration.UpdateBatchSize;
  }


  // Interface methods
  public async Task TrackListEntries(AdList list, HashSet<BlockListEntry> listEntries)
  {
    _logger.LogInformation("Processing {count} domains", listEntries.Count);
    var existingListEntries = (await _domainRepo.GetEntriesAsync(list))
      .ToHashSet();

    var nonDbListEntries = listEntries
      .Where(e => !existingListEntries.Contains(e))
      .ToHashSet();

    var existingDomains = await _domainRepo.GetEntriesByDomain(list,
      nonDbListEntries.Select(x => x.Domain).ToArray());


    Console.WriteLine();
      Console.WriteLine();

    await AddNewEntriesAsync(list, listEntries
      .Where(e => !existingListEntries.Contains(e))
      .ToList());

    await UpdateSeenCountAsync(list, listEntries
      .Where(e => existingListEntries.Contains(e))
      .Select(x => x.Domain)
      .ToList());
  }


  // Internal methods
  private async Task AddNewEntriesAsync(AdList list, IReadOnlyCollection<BlockListEntry> domains)
  {
    var domainsCount = domains.Count;
    if (domainsCount == 0)
      return;

    var batch = new List<BlockListEntry>();
    var addedCount = 0;
    var startTime = DateTime.Now;

    foreach (BlockListEntry domain in domains)
    {
      batch.Add(domain);
      if (batch.Count < _insertBatchSize)
        continue;

      addedCount += batch.Count;
      Console.Write($"\rAdding {batch.Count} new entries to {list} " +
                    $"({domainsCount - addedCount} remaining) " +
                    $"({addedCount} added) " +
                    $"in {(DateTime.Now - startTime).TotalSeconds} seconds.");
      await _domainRepo.AddEntriesAsync(list, batch);
      batch.Clear();
    }

    if (batch.Count == 0)
    {
      Console.WriteLine();
      return;
    }

    addedCount += batch.Count;
    Console.Write($"\rAdding {batch.Count} new entries to {list} " +
                  $"({domainsCount - addedCount} remaining) " +
                  $"({addedCount} added) " +
                  $"in {(DateTime.Now - startTime).TotalSeconds} seconds.");
    await _domainRepo.AddEntriesAsync(list, batch);
    Console.WriteLine();
  }

  private async Task UpdateSeenCountAsync(AdList list, IReadOnlyCollection<string> domains)
  {
    if (domains.Count == 0)
      return;

    var batch = new List<string>();

    foreach (var entry in domains)
    {
      batch.Add(entry);

      if (batch.Count < _updateBatchSize)
        continue;

      _logger.LogInformation("Updating {count} new entries to {list}", batch.Count, list);
      await _domainRepo.UpdateSeenCountAsync(list, batch.ToArray());
      batch.Clear();
    }

    if (batch.Count == 0)
      return;

    _logger.LogInformation("Updating {count} new entries to {list}", batch.Count, list);
    await _domainRepo.UpdateSeenCountAsync(list, batch.ToArray());
  }
}
