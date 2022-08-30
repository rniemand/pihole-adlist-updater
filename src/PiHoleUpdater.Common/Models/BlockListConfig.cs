using YamlDotNet.Serialization;

namespace PiHoleUpdater.Common.Models;

public class BlockListConfig
{
  [YamlMember(Alias = "url")]
  public string ListUrl { get; set; } = string.Empty;

  [YamlMember(Alias = "restrictive")]
  public bool Restrictive { get; set; }
}
