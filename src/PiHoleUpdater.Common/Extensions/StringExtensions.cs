namespace PiHoleUpdater.Common.Extensions;

public static class StringExtensions
{
  public static bool IgnoreCaseEquals(this string str, string value) =>
    str.Equals(value, StringComparison.InvariantCultureIgnoreCase);
}
