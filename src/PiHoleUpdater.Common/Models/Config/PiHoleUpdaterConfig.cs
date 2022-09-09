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
  
  [YamlMember(Alias = "block_lists")]
  public BlockListCategoryConfig[] BlockLists { get; set; } = Array.Empty<BlockListCategoryConfig>();

  [YamlMember(Alias = "whitelist")]
  public WhiteListConfig Whitelist { get; set; } = new();
}
