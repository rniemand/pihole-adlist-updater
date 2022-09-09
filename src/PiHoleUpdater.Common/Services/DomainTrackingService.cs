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
  private readonly int _domainLookupBatchSize;

  public DomainTrackingService(ILoggerAdapter<DomainTrackingService> logger,
    IDomainRepo domainRepo,
    PiHoleUpdaterConfig config)
  {
    _logger = logger;
    _domainRepo = domainRepo;
    _insertBatchSize = config.ListGeneration.InsertBatchSize;
    _updateBatchSize = config.ListGeneration.UpdateBatchSize;
    _domainLookupBatchSize = config.ListGeneration.DomainLookupBatchSize;
  }


  // Interface methods
  public async Task TrackListEntries(AdList list, HashSet<BlockListEntry> listEntries)
  {
    _logger.LogInformation("Processing {count} domains", listEntries.Count);
    var existingDbEntries = (await _domainRepo.GetEntriesAsync(list)).ToHashSet();

    // Find all entries that do not exists for "list" in the DB
    var newListEntries = listEntries
      .Where(e => !existingDbEntries.Contains(e))
      .ToHashSet();

    // Check for, and assign any domains that exist in any other list
    var commonListEntries = await FindExistingDomainsAsync(list, newListEntries);
    await HandleCommonListEntriesAsync(list, commonListEntries);

    // Find all domains that do not exist anywhere in the DB, and add them
    var dbEntriesToAdd = newListEntries
      .Where(e => !commonListEntries.Contains(e))
      .ToHashSet();

    await AddNewEntriesAsync(list, dbEntriesToAdd);

    // Update the seen count for all existing DB entries
    await UpdateSeenCountAsync(list, listEntries
      .Where(e => !dbEntriesToAdd.Contains(e))
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

    foreach (var domain in domains)
    {
      batch.Add(domain);
      if (batch.Count < _insertBatchSize)
        continue;

      addedCount += batch.Count;
      Console.Write($"\r > Adding {batch.Count} new entries to {list} " +
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
    Console.Write($"\r > Adding {batch.Count} new entries to {list} " +
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
    var updatedCount = 0;
    var startTime = DateTime.Now;

    foreach (var entry in domains)
    {
      batch.Add(entry);
      if (batch.Count < _updateBatchSize)
        continue;

      updatedCount += batch.Count;
      Console.Write($"\r > Updating seen count for {batch.Count} entries " +
                    $"({updatedCount} of {domains.Count}) " +
                    $"{domains.Count - updatedCount} remaining " +
                    $"in {(DateTime.Now - startTime).TotalSeconds} seconds");

      await _domainRepo.UpdateSeenCountAsync(batch.ToArray());
      batch.Clear();
    }

    if (batch.Count == 0)
    {
      Console.WriteLine();
      return;
    }

    updatedCount += batch.Count;
    Console.Write($"\r > Updating seen count for {batch.Count} entries " +
                  $"({updatedCount} of {domains.Count}) " +
                  $"{domains.Count - updatedCount} remaining " +
                  $"in {(DateTime.Now - startTime).TotalSeconds} seconds");
    Console.WriteLine();

    await _domainRepo.UpdateSeenCountAsync(batch.ToArray());
  }

  private async Task<HashSet<BlockListEntry>> FindExistingDomainsAsync(AdList list, IReadOnlyCollection<BlockListEntry> listEntries)
  {
    if (listEntries.Count == 0)
      return new HashSet<BlockListEntry>();

    _logger.LogInformation("Looking for common domains for list: {list}", list);
    var domains = listEntries.Select(x => x.Domain).ToArray();
    var existingEntries = new HashSet<BlockListEntry>();
    var batchDomains = new List<string>();

    foreach (var domain in domains)
    {
      batchDomains.Add(domain);
      if (batchDomains.Count < _domainLookupBatchSize)
        continue;

      var dbDomains = (await _domainRepo.GetEntriesByDomain(list, batchDomains.ToArray())).ToList();
      batchDomains.Clear();
      if (dbDomains.Count == 0)
        continue;

      foreach (var dbDomain in dbDomains)
        existingEntries.Add(dbDomain);

      Console.Write($"\r > Found {dbDomains.Count} common domain(s) from other lists " +
                    $"- found {existingEntries.Count} shared domain(s) in total");
    }

    if (batchDomains.Count > 0)
    {
      var dbDomains = (await _domainRepo.GetEntriesByDomain(list, batchDomains.ToArray())).ToList();
      if (dbDomains.Count > 0)
      {
        foreach (var dbDomain in dbDomains)
          existingEntries.Add(dbDomain);
        Console.Write($"\r > Found {dbDomains.Count} common domain(s) from other lists " +
                      $"- found {existingEntries.Count} shared domain(s) in total");
      }
    }

    Console.WriteLine();

    // Log and return all found entries
    _logger.LogInformation("Found {count} existing domain entries for list: {list}",
      existingEntries.Count,
      list);

    return existingEntries;
  }

  private async Task HandleCommonListEntriesAsync(AdList list, IReadOnlyCollection<BlockListEntry> entries)
  {
    if (entries.Count == 0)
      return;

    _logger.LogTrace("Flagging {count} common list entries for list: {list}",
      entries.Count,
      list);

    var domains = entries.Select(x => x.Domain).ToArray();
    var rowCount = await _domainRepo.AssignDomainsToListAsync(list, domains);

    _logger.LogDebug("Updated {count} common domains for list: {list}",
      rowCount,
      list);
  }
}
