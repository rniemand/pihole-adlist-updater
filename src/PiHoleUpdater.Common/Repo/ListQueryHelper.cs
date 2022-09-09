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
      _ => throw new ArgumentOutOfRangeException(nameof(list), list, null)
    };
  }

  public static string GenerateSelectColumnName(AdList list)
  {
    return list switch
    {
      AdList.Suspicious => "'Suspicious' as `ListName`",
      AdList.Advertising => "'Advertising' as `ListName`",
      AdList.Tracking => "'Tracking' as `ListName`",
      AdList.Malicious => "'Malicious' as `ListName`",
      AdList.Adult => "'Adult' as `ListName`",
      AdList.Other => "'Other' as `ListName`",
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
      _ => throw new ArgumentOutOfRangeException(str)
    };
  }

  public static string StringFromAdList(AdList list) =>
    list.ToString("G").ToLower().Trim();
}
