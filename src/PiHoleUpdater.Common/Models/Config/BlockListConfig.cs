using YamlDotNet.Serialization;

namespace PiHoleUpdater.Common.Models.Config;

public class BlockListConfig
{
  [YamlMember(Alias = "name")]
  public string Name
  {
    get => _name;
    set
    {
      if (string.IsNullOrWhiteSpace(value))
        value = string.Empty;
      _name = value.ToLower().Trim();
    }
  }

  [YamlIgnore]
  private string _name = string.Empty;

  [YamlMember(Alias = "enabled")]
  public bool Enabled { get; set; } = true;

  [YamlMember(Alias = "entries")]
  public BlockListConfigEntry[] Entries { get; set; } = Array.Empty<BlockListConfigEntry>();
}
