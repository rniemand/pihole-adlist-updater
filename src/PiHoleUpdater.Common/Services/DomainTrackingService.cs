using PiHoleUpdater.Common.Logging;
using PiHoleUpdater.Common.Models.Config;
using PiHoleUpdater.Common.Models.Repo;
using PiHoleUpdater.Common.Repo;

namespace PiHoleUpdater.Common.Services;

public interface IDomainTrackingService
{
  Task TrackListEntries(string listName, HashSet<BlockListEntry> listEntries);
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
  public async Task TrackListEntries(string listName, HashSet<BlockListEntry> listEntries)
  {
    _logger.LogInformation("Processing {count} domains", listEntries.Count);
    var dbEntries = (await _domainRepo.GetEntriesAsync(listName.Trim()))
      .ToHashSet();

    await AddNewEntriesAsync(listName, listEntries
      .Where(e => !dbEntries.Contains(e))
      .ToList());

    await UpdateSeenCountAsync(listName, listEntries
      .Where(e => dbEntries.Contains(e))
      .Select(x => x.Domain)
      .ToList());

    await DeleteEntriesAsync(listName, dbEntries
      .Where(e => !listEntries.Contains(e))
      .Select(x => x.Domain)
      .ToList());
  }


  // Internal methods
  private async Task AddNewEntriesAsync(string listName, IReadOnlyCollection<BlockListEntry> domains)
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
        batch.Count, listName, (domains.Count - addedCount));
      await _domainRepo.AddEntriesAsync(batch);
      batch.Clear();
    }

    if (batch.Count == 0)
      return;

    addedCount += batch.Count;
    _logger.LogInformation("Adding {count} new entries to {list} ({rem} remaining)",
      batch.Count, listName, (domains.Count - addedCount));
    await _domainRepo.AddEntriesAsync(batch);
  }

  private async Task UpdateSeenCountAsync(string listName, IReadOnlyCollection<string> domains)
  {
    if (domains.Count == 0)
      return;

    var batch = new List<string>();

    foreach (var entry in domains)
    {
      batch.Add(entry);

      if (batch.Count < _updateBatchSize)
        continue;

      _logger.LogInformation("Updating {count} new entries to {list}", batch.Count, listName);
      await _domainRepo.UpdateSeenCountAsync(listName, batch.ToArray());
      batch.Clear();
    }

    if (batch.Count == 0)
      return;

    _logger.LogInformation("Updating {count} new entries to {list}", batch.Count, listName);
    await _domainRepo.UpdateSeenCountAsync(listName, batch.ToArray());
  }

  private async Task DeleteEntriesAsync(string listName, IReadOnlyCollection<string> domains)
  {
    if (domains.Count == 0)
      return;

    _logger.LogInformation("Removing {count} new entries to {list}", domains.Count, listName);
    await _domainRepo.DeleteEntriesAsync(listName, domains.ToArray());
  }
}
