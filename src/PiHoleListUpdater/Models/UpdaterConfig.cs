using YamlDotNet.Serialization;

namespace PiHoleListUpdater.Models;

class UpdaterConfig
{
  [YamlMember(Alias = "block_lists")]
  public Dictionary<string, BlockListConfig[]> BlockLists { get; set; } = new();

  [YamlMember(Alias = "output_dir")]
  public string OutputDir { get; set; } = string.Empty;

  [YamlMember(Alias = "development")]
  public DevelopmentConfig Development { get; set; } = new();
  
  public class DevelopmentConfig
  {
    [YamlMember(Alias = "enabled")]
    public bool Enabled { get; set; }

    [YamlMember(Alias = "use_cached_lists")]
    public bool UseCachedLists { get; set; }

    [YamlMember(Alias = "capture_responses")]
    public bool CaptureResponses { get; set; }

    [YamlMember(Alias = "cached_response_dir")]
    public string CachedResponseDir { get; set; } = "TestData/";

    [YamlMember(Alias = "capture_response_dir")]
    public string CaptureResponseDir { get; set; } = string.Empty;
  }
}

class BlockListConfig
{
  [YamlMember(Alias = "url")]
  public string ListUrl { get; set; } = string.Empty;

  [YamlMember(Alias = "restrictive")]
  public bool Restrictive { get; set; }
}
