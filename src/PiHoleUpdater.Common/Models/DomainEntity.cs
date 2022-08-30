namespace PiHoleUpdater.Common.Models;

public class DomainEntity
{
  public long DomainId { get; set; }
  public DateOnly DateAdded { get; set; } = DateOnly.FromDateTime(DateTime.Now);
  public DateOnly DateLastSeen { get; set; } = DateOnly.FromDateTime(DateTime.Now);
  public DateTime? DateDeleted { get; set; }
  public bool Deleted { get; set; }
  public bool Restrictive { get; set; }
  public int SeenCount { get; set; }
  public string Domain { get; set; } = string.Empty;
  public string ListName { get; set; } = string.Empty;
}
