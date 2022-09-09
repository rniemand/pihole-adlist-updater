using PiHoleUpdater.Common.Enums;
using PiHoleUpdater.Common.Repo;
using YamlDotNet.Serialization;

namespace PiHoleUpdater.Common.Models.Config;

public class BlockListCategoryConfig
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

  [YamlMember(Alias = "entries")]
  public BlockListConfigEntry[] Entries { get; set; } = Array.Empty<BlockListConfigEntry>();
}
