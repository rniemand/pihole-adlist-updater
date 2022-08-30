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
    var currentEntries = await _domainRepo.GetListEntries(listName.Trim());




    await Task.CompletedTask;
  }
}
