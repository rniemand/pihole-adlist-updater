namespace PiHoleUpdater.Common.Models.Repo;

public class WhitelistEntity
{
  public int EntryId { get; set; }
  public bool Enabled { get; set; } = true;
  public bool IsRegex { get; set; } = false;
  public int Order { get; set; } = 255;
  public string Expression { get; set; } = string.Empty;
}
