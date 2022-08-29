using System.Text.RegularExpressions;

namespace PiHoleListUpdater;

internal class AdListParser
{
  private static Regex TRIM_LINE_RX = new Regex("((\\d{1,3}\\.){3}\\d{1,}|(\\:[^\\s]+))\\s+",
    RegexOptions.Compiled | RegexOptions.Singleline);

  public List<string> ParseList(string rawList)
  {
    if (string.IsNullOrWhiteSpace(rawList))
      return new List<string>();

    var entries = new List<string>();
    foreach (var line in rawList.Split("\n"))
    {
      if (line.StartsWith("#"))
        continue;

      if (string.IsNullOrWhiteSpace(line))
        continue;

      var cleanLine = TRIM_LINE_RX.Replace(line, "").Trim();
      if (string.IsNullOrWhiteSpace(cleanLine))
        continue;

      entries.Add(cleanLine);
    }

    return entries;
  }
}
