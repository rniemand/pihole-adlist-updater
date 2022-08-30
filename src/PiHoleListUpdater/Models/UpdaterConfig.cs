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

  [YamlMember(Alias = "list_generation")]
  public ListGenerationConfig ListGeneration { get; set; } = new();


  // Internal classes
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

  public class ListGenerationConfig
  {
    [YamlMember(Alias = "category_all")]
    public bool CategoryAll { get; set; }

    [YamlMember(Alias = "category_safe")]
    public bool CategorySafe { get; set; }
  }
}
