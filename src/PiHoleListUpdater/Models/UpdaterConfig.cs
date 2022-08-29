using YamlDotNet.Serialization;

namespace PiHoleListUpdater.Models;

class UpdaterConfig
{
  [YamlMember(Alias = "block_lists")]
  public string[] BlockLists { get; set; } = Array.Empty<string>();
}
