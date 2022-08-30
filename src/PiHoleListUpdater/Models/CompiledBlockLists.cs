namespace PiHoleListUpdater.Models;

class CompiledBlockLists
{
  public Dictionary<string, HashSet<BlockListEntry>> Lists { get; set; } = new();
  public string[] Categories => Lists.Keys.ToArray();

  public int AddEntries(string listCategory, IEnumerable<string> domains, bool restrictive)
  {
    if (string.IsNullOrWhiteSpace(listCategory))
      return 0;

    var addCount = 0;
    var safeCategory = listCategory.ToLower().Trim();

    if (!Lists.ContainsKey(safeCategory))
      Lists[safeCategory] = new HashSet<BlockListEntry>();

    foreach (var domain in domains)
    {
      var entry = new BlockListEntry(domain, restrictive);

      if (Lists[safeCategory].Contains(entry))
        continue;

      Lists[safeCategory].Add(entry);
      addCount++;
    }

    return addCount;
  }

  public List<string> GetListEntries(string listCategory)
  {
    if (string.IsNullOrWhiteSpace(listCategory))
      return new List<string>();

    var safeCategory = listCategory.ToLower().Trim();
    if(!Lists.ContainsKey(safeCategory))
      return new List<string>();

    return Lists[safeCategory]
      .Where(x => !x.Restrictive)
      .Select(x => x.Domain)
      .ToList();
  }

  public List<string> GetAllListEntries(string listCategory)
  {
    if (string.IsNullOrWhiteSpace(listCategory))
      return new List<string>();

    var safeCategory = listCategory.ToLower().Trim();
    if (!Lists.ContainsKey(safeCategory))
      return new List<string>();

    return Lists[safeCategory]
      .Select(x => x.Domain)
      .ToList();
  }
}

struct BlockListEntry
{
  public string Domain { get; set; }
  public bool Restrictive { get; set; }

  public BlockListEntry(string domain, bool restrictive)
  {
    Domain = domain;
    Restrictive = restrictive;
  }
}
