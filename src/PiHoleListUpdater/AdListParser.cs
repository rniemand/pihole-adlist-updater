using System.Text.RegularExpressions;

namespace PiHoleListUpdater;

internal class AdListParser
{
  private static Regex TRIM_LINE_RX = new Regex("((\\d{1,3}\\.){3}\\d{1,}|(\\:[^\\s]+))\\s+",
    RegexOptions.Compiled | RegexOptions.Singleline);

  public void ParseList(HashSet<string> domains, string rawList)
  {
    var addedCount = 0;
    foreach (var line in rawList.Split("\n"))
    {
      if (line.StartsWith("#"))
        continue;

      if (string.IsNullOrWhiteSpace(line))
        continue;

      var cleanLine = TRIM_LINE_RX.Replace(line, "").Trim();
      if (string.IsNullOrWhiteSpace(cleanLine))
        continue;

      if (domains.Contains(cleanLine))
        continue;

      domains.Add(cleanLine);
      addedCount++;
    }

    if (addedCount > 0)
      Console.WriteLine($"Added {addedCount} new domains");
  }
}