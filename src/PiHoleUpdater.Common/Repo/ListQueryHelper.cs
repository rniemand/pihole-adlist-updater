using PiHoleUpdater.Common.Enums;

namespace PiHoleUpdater.Common.Repo;

public static class ListQueryHelper
{
  public static string GenerateWhereFilter(AdListType list, string prefix = "")
  {
    var filter = "";

    if (!string.IsNullOrEmpty(prefix))
      filter = $"{prefix}.";

    return list switch
    {
      AdListType.Suspicious => filter + "`Suspicious` = 1",
      AdListType.Advertising => filter + "`Advertising` = 1",
      AdListType.Tracking => filter + "`Tracking` = 1",
      AdListType.Malicious => filter + "`Malicious` = 1",
      AdListType.Adult => filter + "`Adult` = 1",
      AdListType.Other => filter + "`Other` = 1",
      AdListType.Spam => filter + "`Spam` = 1",
      AdListType.Combined => filter + "`Combined` = 1",
      AdListType.Facebook => filter + "`Facebook` = 1",
      _ => throw new ArgumentOutOfRangeException(nameof(list), list, null)
    };
  }

  public static string GenerateSelectColumnName(AdListType list)
  {
    return list switch
    {
      AdListType.Suspicious => "'suspicious' as `ListName`",
      AdListType.Advertising => "'advertising' as `ListName`",
      AdListType.Tracking => "'tracking' as `ListName`",
      AdListType.Malicious => "'malicious' as `ListName`",
      AdListType.Adult => "'adult' as `ListName`",
      AdListType.Other => "'other' as `ListName`",
      AdListType.Spam => "'spam' as `ListName`",
      AdListType.Combined => "'combined' as `ListName`",
      AdListType.Facebook => "'facebook' as `ListName`",
      _ => throw new ArgumentOutOfRangeException(nameof(list), list, null)
    };
  }

  public static string GetColumnName(AdListType list)
  {
    return list switch
    {
      AdListType.Suspicious => "Suspicious",
      AdListType.Advertising => "Advertising",
      AdListType.Tracking => "Tracking",
      AdListType.Malicious => "Malicious",
      AdListType.Adult => "Adult",
      AdListType.Other => "Other",
      AdListType.Spam => "Spam",
      AdListType.Combined => "Combined",
      AdListType.Facebook => "Facebook",
      _ => throw new ArgumentOutOfRangeException(nameof(list), list, null)
    };
  }

  public static AdListType AdListTypeFromString(string str)
  {
    return str.ToLower().Trim() switch
    {
      "suspicious" => AdListType.Suspicious,
      "advertising" => AdListType.Advertising,
      "tracking" => AdListType.Tracking,
      "malicious" => AdListType.Malicious,
      "adult" => AdListType.Adult,
      "other" => AdListType.Other,
      "spam" => AdListType.Spam,
      "unknown" => AdListType.Unknown,
      "combined" => AdListType.Combined,
      "facebook" => AdListType.Facebook,
      _ => throw new ArgumentOutOfRangeException(str)
    };
  }

  public static string StringFromAdList(AdListType list) =>
    list.ToString("G").ToLower().Trim();
}
