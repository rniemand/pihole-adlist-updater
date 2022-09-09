using PiHoleUpdater.Common.Models.Config;
using PiHoleUpdater.Common.Models.Repo;
using PiHoleUpdater.Common.Repo;
using System.Text.RegularExpressions;
using PiHoleUpdater.Common.Enums;

namespace PiHoleUpdater.Common.Utils;

public interface IBlockListParser
{
  int AppendNewEntries(HashSet<BlockListEntry> domains, AdList list, string rawList);
}

public class BlockListParser : IBlockListParser
{
  private static readonly Regex TrimLineRx = new("((\\d{1,3}\\.){3}\\d{1,}|(\\:[^\\s]+))\\s+",
    RegexOptions.Compiled | RegexOptions.Singleline);

  private readonly PiHoleUpdaterConfig _config;

  public BlockListParser(PiHoleUpdaterConfig config)
  {
    _config = config;
  }

  public int AppendNewEntries(HashSet<BlockListEntry> domains, AdList list, string rawList)
  {
    if (string.IsNullOrWhiteSpace(rawList))
      return 0;

    var listName = ListQueryHelper.StringFromAdList(list);
    var wlRegex = _config.Whitelist.CompiledRegex;
    var wlExact = _config.Whitelist.ExactDomains;
    var addedCount = 0;

    foreach (var line in rawList.Split("\n", StringSplitOptions.RemoveEmptyEntries))
    {
      if (string.IsNullOrWhiteSpace(line))
        continue;

      var trimmedLIne = line.Trim();

      if (trimmedLIne.StartsWith("#") || trimmedLIne.StartsWith("|") || trimmedLIne.EndsWith("^"))
        continue;

      var cleanLine = TrimLineRx.Replace(trimmedLIne, "").Trim();
      if (string.IsNullOrWhiteSpace(cleanLine))
        continue;

      if (wlExact.Any(x => x.Equals(cleanLine, StringComparison.InvariantCultureIgnoreCase)))
        continue;

      if (wlRegex.Any(x => x.IsMatch(cleanLine)))
        continue;

      var blockListEntry = new BlockListEntry
      {
        ListName = listName,
        Domain = cleanLine
      };

      if (domains.Contains(blockListEntry))
        continue;

      domains.Add(blockListEntry);
      addedCount++;
    }

    return addedCount;
  }
}
