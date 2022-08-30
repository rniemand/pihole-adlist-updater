namespace PiHoleUpdater.Common.Models;

public struct BlockListEntry
{
  public string Domain { get; set; }
  public bool Restrictive { get; set; }

  public BlockListEntry(string domain, bool restrictive)
  {
    Domain = domain;
    Restrictive = restrictive;
  }
}
