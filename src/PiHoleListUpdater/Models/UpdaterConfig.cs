using System.Text.RegularExpressions;
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

  [YamlMember(Alias = "whitelist")]
  public WhiteListConfig Whitelist { get; set; } = new();

  [YamlMember(Alias = "generate_all_list")]
  public bool GenerateAllList { get; set; }

  [YamlMember(Alias = "generate_safe_list")]
  public bool GenerateSafeList { get; set; }
  
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

  public class WhiteListConfig
  {
    [YamlMember(Alias = "regex")]
    public string[] RegexPatterns { get; set; } = Array.Empty<string>();

    [YamlIgnore]
    public Regex[] CompiledRegex { get; set; } = Array.Empty<Regex>();

    [YamlMember(Alias = "exact")]
    public string[] ExactDomains { get; set; } = Array.Empty<string>();
  }
}
