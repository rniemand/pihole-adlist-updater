using System.Text.RegularExpressions;
using PiHoleListUpdater.Models;

namespace PiHoleListUpdater;

internal class BlockListParser
{
  private static Regex TRIM_LINE_RX = new Regex("((\\d{1,3}\\.){3}\\d{1,}|(\\:[^\\s]+))\\s+",
    RegexOptions.Compiled | RegexOptions.Singleline);

  private readonly UpdaterConfig _config;

  public BlockListParser(UpdaterConfig config)
  {
    _config = config;
  }

  public List<string> ParseList(string rawList)
  {
    if (string.IsNullOrWhiteSpace(rawList))
      return new List<string>();

    var entries = new List<string>();
    var wlRegex = _config.Whitelist.CompiledRegex;
    var wlExact = _config.Whitelist.ExactDomains;

    foreach (var line in rawList.Split("\n"))
    {
      if (line.StartsWith("#"))
        continue;

      if (string.IsNullOrWhiteSpace(line))
        continue;

      var cleanLine = TRIM_LINE_RX.Replace(line, "").Trim();
      if (string.IsNullOrWhiteSpace(cleanLine))
        continue;

      if (wlExact.Any(x => x.Equals(cleanLine, StringComparison.InvariantCultureIgnoreCase)))
      {
        Console.WriteLine($"  - skipping {cleanLine} (exact match)");
        continue;
      }

      if (wlRegex.Any(x => x.IsMatch(cleanLine)))
      {
        Console.WriteLine($"  - skipping {cleanLine} (regex match)");
        continue;
      }

      entries.Add(cleanLine);
    }

    return entries;
  }
}
