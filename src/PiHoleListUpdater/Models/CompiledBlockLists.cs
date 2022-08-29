namespace PiHoleListUpdater.Models;

class CompiledBlockLists
{
  public Dictionary<string, HashSet<string>> Lists { get; set; } = new();
  public string[] Categories => Lists.Keys.ToArray();

  public int AddEntries(string listCategory, IEnumerable<string> domains)
  {
    if (string.IsNullOrWhiteSpace(listCategory))
      return 0;

    var addCount = 0;
    var safeCategory = listCategory.ToLower().Trim();

    if (!Lists.ContainsKey(safeCategory))
      Lists[safeCategory] = new HashSet<string>();


    foreach (var domain in domains)
    {
      if (Lists[safeCategory].Contains(domain))
        continue;

      Lists[safeCategory].Add(domain);
      addCount++;
    }

    return addCount;
  }
}
