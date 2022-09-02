using YamlDotNet.Serialization;

namespace PiHoleUpdater.Common.Models.Config;

public class PiHoleUpdaterConfig
{
  [YamlMember(Alias = "output_dir")]
  public string OutputDir { get; set; } = string.Empty;

  [YamlMember(Alias = "local_repo")]
  public string LocalRepo { get; set; } = string.Empty;

  [YamlMember(Alias = "db_connection_string")]
  public string DbConnectionString { get; set; } = string.Empty;

  [YamlMember(Alias = "list_generation")]
  public ListGenerationConfig ListGeneration { get; set; } = new();

  [YamlMember(Alias = "development")]
  public DevelopmentConfig Development { get; set; } = new();

  [YamlMember(Alias = "block_lists")]
  public BlockListCategoryConfig[] BlockLists { get; set; } = Array.Empty<BlockListCategoryConfig>();

  [YamlMember(Alias = "whitelist")]
  public WhiteListConfig Whitelist { get; set; } = new();
}
