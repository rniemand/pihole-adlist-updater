using PiHoleUpdater.Common.Enums;
using PiHoleUpdater.Common.Repo;
using YamlDotNet.Serialization;

namespace PiHoleUpdater.Common.Models.Config;

public class AdListCategoryConfig
{
  [YamlMember(Alias = "name")]
  public AdList Name
  {
    get => ListQueryHelper.AdListFromString(_name);
    set => _name = value.ToString("G").ToLower().Trim();
  }

  [YamlIgnore]
  private string _name = string.Empty;

  [YamlMember(Alias = "enabled")]
  public bool Enabled { get; set; } = true;

  [YamlMember(Alias = "sources")]
  public AdListSourceConfig[] Sources { get; set; } = Array.Empty<AdListSourceConfig>();
}
