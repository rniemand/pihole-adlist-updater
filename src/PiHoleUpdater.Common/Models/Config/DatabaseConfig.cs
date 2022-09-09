using YamlDotNet.Serialization;

namespace PiHoleUpdater.Common.Models.Config;

public class DatabaseConfig
{
  [YamlMember(Alias = "connection_string")]
  public string ConnectionString { get; set; } = string.Empty;

  [YamlMember(Alias = "insert_batch_size")]
  public int InsertBatchSize { get; set; } = 10000;

  [YamlMember(Alias = "update_batch_size")]
  public int UpdateBatchSize { get; set; } = 20000;

  [YamlMember(Alias = "lookup_batch_size")]
  public int LookupBatchSize { get; set; } = 20000;
}
