using PiHoleUpdater.Common.Enums;

namespace PiHoleUpdater.Common.Models.Repo;

public class AdListEntity
{
  public int AdListId { get; set; }
  public bool Enabled { get; set; } = true;
  public AdListType AdListType { get; set; } = AdListType.Unknown;
  public AdListSource AdListSource { get; set; } = AdListSource.Unknown;
  public string ListUrl { get; set; } = string.Empty;
  public string ProjectUrl { get; set; } = string.Empty;
  public string Maintainer { get; set; } = string.Empty;
}
