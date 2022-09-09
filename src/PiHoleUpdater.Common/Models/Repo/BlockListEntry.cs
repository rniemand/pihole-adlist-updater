namespace PiHoleUpdater.Common.Models.Repo;

public struct BlockListEntry
{
  public string Domain { get; set; }
  public string ListName { get; set; }

  public BlockListEntry(string listName, string domain)
  {
    ListName = listName;
    Domain = domain;
  }
}
