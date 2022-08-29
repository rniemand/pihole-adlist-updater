using YamlDotNet.Serialization;

namespace PiHoleListUpdater.Models;

class UpdaterConfig
{
  [YamlMember(Alias = "block_lists")]
  public string[] BlockLists { get; set; } = Array.Empty<string>();

  [YamlMember(Alias = "outputs")]
  public OutputsConfig Outputs { get; set; } = new();

  [YamlMember(Alias = "development")]
  public DevelopmentConfig Development { get; set; } = new();

  public class OutputsConfig
  {
    [YamlMember(Alias = "everything")]
    public string Everything { get; set; } = string.Empty;
  }

  public class DevelopmentConfig
  {
    [YamlMember(Alias = "enabled")]
    public bool Enabled { get; set; }

    [YamlMember(Alias = "use_cached_lists")]
    public bool UseCachedLists { get; set; }

    [YamlMember(Alias = "cached_response_dir")]
    public string CachedResponseDir { get; set; } = "TestData/";
  }
}
