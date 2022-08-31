using YamlDotNet.Serialization;

namespace PiHoleUpdater.Common.Models.Config;

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
