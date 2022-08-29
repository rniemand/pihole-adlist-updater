using YamlDotNet.Serialization;

namespace PiHoleListUpdater.Models;

class UpdaterConfig
{
  [YamlMember(Alias = "block_lists")]
  public string[] BlockLists { get; set; } = Array.Empty<string>();

  [YamlMember(Alias = "outputs")]
  public OutputsConfig Outputs { get; set; } = new();

  public class OutputsConfig
  {
    [YamlMember(Alias = "everything")]
    public string Everything { get; set; } = string.Empty;
  }
}
