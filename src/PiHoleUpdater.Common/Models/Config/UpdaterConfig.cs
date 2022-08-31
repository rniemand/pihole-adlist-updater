using YamlDotNet.Serialization;

namespace PiHoleUpdater.Common.Models.Config;

public class UpdaterConfig
{
  [YamlMember(Alias = "block_lists")]
  public BlockListConfig[] BlockLists { get; set; } = Array.Empty<BlockListConfig>();

  [YamlMember(Alias = "output_dir")]
  public string OutputDir { get; set; } = string.Empty;

  [YamlMember(Alias = "development")]
  public DevelopmentConfig Development { get; set; } = new();

  [YamlMember(Alias = "whitelist")]
  public WhiteListConfig Whitelist { get; set; } = new();

  [YamlMember(Alias = "list_generation")]
  public ListGenerationConfig ListGeneration { get; set; } = new();

  [YamlMember(Alias = "db_connection_string")]
  public string DbConnectionString { get; set; } = string.Empty;
}
