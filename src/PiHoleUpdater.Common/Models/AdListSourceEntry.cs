using PiHoleUpdater.Common.Enums;

namespace PiHoleUpdater.Common.Models;

public class AdListSourceEntry
{
  public string ListUrl { get; set; } = string.Empty;

  public AdListType SourceType { get; set; } = AdListType.Unknown;

  public string ProjectUrl { get; set; } = string.Empty;

  public string Maintainer { get; set; } = string.Empty;

  public bool Enabled { get; set; } = true;
}
