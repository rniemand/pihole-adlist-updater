namespace PiHoleListUpdater.Models;

class CompiledBlockLists
{
  public Dictionary<string, HashSet<BlockListEntry>> Lists { get; set; } = new();
  public string[] Categories => Lists.Keys.ToArray();

  public int AddDomains(string listCategory, IEnumerable<string> domains, bool restrictive)
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

  public List<string> GetSafeEntries()
  {
    return Lists
      .SelectMany(x => x.Value)
      .Where(x => !x.Restrictive)
      .Select(x => x.Domain)
      .OrderBy(x => x)
      .Distinct()
      .ToList();
  }

  public List<string> GetSafeEntries(string listCategory)
  {
    if (string.IsNullOrWhiteSpace(listCategory))
      return new List<string>();

    var safeCategory = listCategory.ToLower().Trim();
    if (!Lists.ContainsKey(safeCategory))
      return new List<string>();

    return Lists[safeCategory]
      .Where(x => !x.Restrictive)
      .Select(x => x.Domain)
      .ToList();
  }

  public List<string> GetAllEntries()
  {
    return Lists
      .SelectMany(x => x.Value)
      .Select(x => x.Domain)
      .OrderBy(x => x)
      .Distinct()
      .ToList();
  }

  public List<string> GetAllEntries(string listCategory)
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
