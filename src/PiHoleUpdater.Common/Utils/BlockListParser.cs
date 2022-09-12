using PiHoleUpdater.Common.Models.Repo;
using PiHoleUpdater.Common.Repo;
using System.Text.RegularExpressions;
using PiHoleUpdater.Common.Enums;
using PiHoleUpdater.Common.Models;

namespace PiHoleUpdater.Common.Utils;

public interface IBlockListParser
{
  Task<int> AppendNewEntries(HashSet<BlockListEntry> domains, AdListType list, string rawList);
}

public class BlockListParser : IBlockListParser
{
  private static readonly Regex TrimLineRx = new("((\\d{1,3}\\.){3}\\d{1,}|(\\:[^\\s]+))\\s+",
    RegexOptions.Compiled | RegexOptions.Singleline);

  private readonly IWhitelistRepo _whitelistRepo;

  public BlockListParser(IWhitelistRepo whitelistRepo)
  {
    _whitelistRepo = whitelistRepo;
  }


  // Public methods
  public async Task<int> AppendNewEntries(HashSet<BlockListEntry> domains, AdListType list, string rawList)
  {
    if (string.IsNullOrWhiteSpace(rawList))
      return 0;

    var rawWhitelist = await GetWhitelistAsync();
    var listName = ListQueryHelper.StringFromAdList(list);

    var wlRegex = rawWhitelist
      .Where(x => x.IsRegex)
      .Select(x => new Regex(x.Value, RegexOptions.Compiled | RegexOptions.Singleline))
      .ToList();

    var wlExact = rawWhitelist
      .Where(x => !x.IsRegex)
      .Select(x => x.Value)
      .ToList();

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

      if (cleanLine.Contains(' '))
        cleanLine = cleanLine.Split(" ")[0];

      if (cleanLine.Contains('#'))
        cleanLine = cleanLine.Split("#")[0];

      if (cleanLine.Contains('@'))
        cleanLine = cleanLine.Split("@")[0];

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


  // Internal methods
  private async Task<List<WhitelistExpression>> GetWhitelistAsync()
  {
    var dbEntries = await _whitelistRepo.GetEntriesAsync();

    return dbEntries
      .Select(entry => new WhitelistExpression
      {
        Value = entry.Expression,
        IsRegex = entry.IsRegex,

      })
      .ToList();
  }
}
