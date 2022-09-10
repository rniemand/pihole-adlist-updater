using YamlDotNet.Serialization;

namespace PiHoleUpdater.Common.Models.Config;

public class PiHoleUpdaterConfig
{
  [YamlMember(Alias = "paths")]
  public PathsConfig Paths { get; set; } = new();

  [YamlMember(Alias = "database")]
  public DatabaseConfig Database { get; set; } = new();
  
  [YamlMember(Alias = "list_generation")]
  public ListGenerationConfig ListGeneration { get; set; } = new();
  
  [YamlMember(Alias = "ad_lists")]
  public AdListCategoryConfig[] AdListCategories { get; set; } = Array.Empty<AdListCategoryConfig>();

  [YamlMember(Alias = "whitelist")]
  public WhiteListConfig Whitelist { get; set; } = new();
}
