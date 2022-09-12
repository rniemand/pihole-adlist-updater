namespace PiHoleUpdater.Common.Models;

public class WhitelistExpression
{
  public bool IsRegex { get; set; }
  public string Value { get; set; } = string.Empty;
}
