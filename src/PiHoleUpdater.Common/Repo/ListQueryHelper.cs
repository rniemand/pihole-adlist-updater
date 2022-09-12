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
      AdListType.Suspicious => filter + "`SuspiciousList` = 1",
      AdListType.Advertising => filter + "`AdvertisingList` = 1",
      AdListType.Tracking => filter + "`TrackingList` = 1",
      AdListType.Malicious => filter + "`MaliciousList` = 1",
      AdListType.Adult => filter + "`AdultList` = 1",
      AdListType.Other => filter + "`OtherList` = 1",
      AdListType.Spam => filter + "`SpamList` = 1",
      AdListType.Combined => filter + "`CombinedList` = 1",
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
      _ => throw new ArgumentOutOfRangeException(nameof(list), list, null)
    };
  }

  public static string GetColumnName(AdListType list)
  {
    return list switch
    {
      AdListType.Suspicious => "SuspiciousList",
      AdListType.Advertising => "AdvertisingList",
      AdListType.Tracking => "TrackingList",
      AdListType.Malicious => "MaliciousList",
      AdListType.Adult => "AdultList",
      AdListType.Other => "OtherList",
      AdListType.Spam => "SpamList",
      AdListType.Combined => "CombinedList",
      _ => throw new ArgumentOutOfRangeException(nameof(list), list, null)
    };
  }

  public static AdListType AdListFromString(string str)
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
      _ => throw new ArgumentOutOfRangeException(str)
    };
  }

  public static string StringFromAdList(AdListType list) =>
    list.ToString("G").ToLower().Trim();
}
