using YamlDotNet.Serialization;

namespace PiHoleUpdater.Common.Models.Config;

public class PathsConfig
{
  [YamlMember(Alias = "lists_output")]
  public string ListsOutput { get; set; } = string.Empty;

  [YamlMember(Alias = "lists_repo")]
  public string ListsRepo { get; set; } = string.Empty;
}
