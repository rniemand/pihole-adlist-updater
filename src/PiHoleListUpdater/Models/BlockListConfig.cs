using YamlDotNet.Serialization;

namespace PiHoleListUpdater.Models;

class BlockListConfig
{
  [YamlMember(Alias = "url")]
  public string ListUrl { get; set; } = string.Empty;

  [YamlMember(Alias = "restrictive")]
  public bool Restrictive { get; set; }
}
