namespace PiHoleUpdater.Common.Models;

public struct BlockListEntry
{
  public string Domain { get; set; }
  public bool Restrictive { get; set; }
  public string ListName { get; set; }

  public BlockListEntry(string listName, string domain, bool restrictive)
  {
    ListName = listName;
    Domain = domain;
    Restrictive = restrictive;
  }
}
