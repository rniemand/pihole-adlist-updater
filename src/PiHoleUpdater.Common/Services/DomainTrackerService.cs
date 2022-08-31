using PiHoleUpdater.Common.Extensions;
using PiHoleUpdater.Common.Logging;
using PiHoleUpdater.Common.Models;
using PiHoleUpdater.Common.Repo;

namespace PiHoleUpdater.Common.Services;

public interface IDomainTrackerService
{
  Task TrackListEntries(string listName, HashSet<BlockListEntry> entries);
}

public class DomainTrackerService : IDomainTrackerService
{
  private readonly ILoggerAdapter<DomainTrackerService> _logger;
  private readonly IDomainRepo _domainRepo;

  public DomainTrackerService(ILoggerAdapter<DomainTrackerService> logger,
    IDomainRepo domainRepo)
  {
    _logger = logger;
    _domainRepo = domainRepo;
  }

  public async Task TrackListEntries(string listName, HashSet<BlockListEntry> entries)
  {
    _logger.LogInformation("Processing {count} domains", entries.Count);
    _logger.LogDebug("Fetching current entries for list: {list}", listName);
    var dbEntriesHash = (await _domainRepo.GetListEntries(listName.Trim()))
      .ToHashSet();
    
    var newEntries = entries.Where(e => !dbEntriesHash.Contains(e)).ToList();
    if (newEntries.Count > 0)
    {
      _logger.LogInformation("Adding {count} new entries to {list}", newEntries.Count, listName);
      await _domainRepo.AddNewEntries(newEntries);
    }

    var existingEntries = entries.Where(e => dbEntriesHash.Contains(e)).Select(x => x.Domain).ToArray();
    if (existingEntries.Length > 0)
    {
      _logger.LogInformation("Updating {count} new entries to {list}", existingEntries.Length, listName);
      await _domainRepo.UpdateSeen(listName, existingEntries);
    }


    await Task.CompletedTask;
  }

}
