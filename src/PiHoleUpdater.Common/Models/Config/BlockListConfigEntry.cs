using YamlDotNet.Serialization;

namespace PiHoleUpdater.Common.Models.Config;

public class BlockListConfigEntry
{
  [YamlMember(Alias = "url")]
  public string Url { get; set; } = string.Empty;

  [YamlMember(Alias = "strict")]
  public bool Strict { get; set; }
}
