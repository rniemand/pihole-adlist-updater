using PiHoleUpdater.Common.Enums;

namespace PiHoleUpdater.Common.Repo;

public static class ListQueryHelper
{
  public static string GenerateWhereFilter(AdList list, string prefix = "")
  {
    var filter = "";

    if (!string.IsNullOrEmpty(prefix))
      filter = $"{prefix}.";

    return list switch
    {
      AdList.Suspicious => filter + "`SuspiciousList` = 1",
      AdList.Advertising => filter + "`AdvertisingList` = 1",
      AdList.Tracking => filter + "`TrackingList` = 1",
      AdList.Malicious => filter + "`MaliciousList` = 1",
      AdList.Adult => filter + "`AdultList` = 1",
      AdList.Other => filter + "`OtherList` = 1",
      AdList.Spam => filter + "`SpamList` = 1",
      AdList.Combined => filter + "`CombinedList` = 1",
      _ => throw new ArgumentOutOfRangeException(nameof(list), list, null)
    };
  }

  public static string GenerateSelectColumnName(AdList list)
  {
    return list switch
    {
      AdList.Suspicious => "'suspicious' as `ListName`",
      AdList.Advertising => "'advertising' as `ListName`",
      AdList.Tracking => "'tracking' as `ListName`",
      AdList.Malicious => "'malicious' as `ListName`",
      AdList.Adult => "'adult' as `ListName`",
      AdList.Other => "'other' as `ListName`",
      AdList.Spam => "'spam' as `ListName`",
      AdList.Combined => "'combined' as `ListName`",
      _ => throw new ArgumentOutOfRangeException(nameof(list), list, null)
    };
  }

  public static string GetColumnName(AdList list)
  {
    return list switch
    {
      AdList.Suspicious => "SuspiciousList",
      AdList.Advertising => "AdvertisingList",
      AdList.Tracking => "TrackingList",
      AdList.Malicious => "MaliciousList",
      AdList.Adult => "AdultList",
      AdList.Other => "OtherList",
      AdList.Spam => "SpamList",
      AdList.Combined => "CombinedList",
      _ => throw new ArgumentOutOfRangeException(nameof(list), list, null)
    };
  }

  public static AdList AdListFromString(string str)
  {
    return str.ToLower().Trim() switch
    {
      "suspicious" => AdList.Suspicious,
      "advertising" => AdList.Advertising,
      "tracking" => AdList.Tracking,
      "malicious" => AdList.Malicious,
      "adult" => AdList.Adult,
      "other" => AdList.Other,
      "spam" => AdList.Spam,
      "unknown" => AdList.Unknown,
      "combined" => AdList.Combined,
      _ => throw new ArgumentOutOfRangeException(str)
    };
  }

  public static string StringFromAdList(AdList list) =>
    list.ToString("G").ToLower().Trim();
}
