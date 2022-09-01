namespace PiHoleUpdater.Common.Models.Repo;

public struct BlockListEntry
{
  public string Domain { get; set; }
  public bool Strict { get; set; }
  public string ListName { get; set; }

  public BlockListEntry(string listName, string domain, bool strict)
  {
    ListName = listName;
    Domain = domain;
    Strict = strict;
  }
}
