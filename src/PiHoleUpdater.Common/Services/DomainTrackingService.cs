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
    var dbEntries = (await _domainRepo.GetEntriesAsync(list))
      .ToHashSet();

    await AddNewEntriesAsync(list, listEntries
      .Where(e => !dbEntries.Contains(e))
      .ToList());

    await UpdateSeenCountAsync(list, listEntries
      .Where(e => dbEntries.Contains(e))
      .Select(x => x.Domain)
      .ToList());
  }


  // Internal methods
  private async Task AddNewEntriesAsync(AdList list, IReadOnlyCollection<BlockListEntry> domains)
  {
    if (domains.Count == 0)
      return;

    var batch = new List<BlockListEntry>();
    var addedCount = 0;

    foreach (BlockListEntry domain in domains)
    {
      batch.Add(domain);

      if (batch.Count < _insertBatchSize)
        continue;

      addedCount += batch.Count;
      _logger.LogInformation("Adding {count} new entries to {list} ({rem} remaining)",
        batch.Count, list, (domains.Count - addedCount));
      await _domainRepo.AddEntriesAsync(list, batch);
      batch.Clear();
    }

    if (batch.Count == 0)
      return;

    addedCount += batch.Count;
    _logger.LogInformation("Adding {count} new entries to {list} ({rem} remaining)",
      batch.Count, list, (domains.Count - addedCount));
    await _domainRepo.AddEntriesAsync(list, batch);
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
