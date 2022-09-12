using PiHoleUpdater.Common.Enums;
using YamlDotNet.Serialization;

namespace PiHoleUpdater.Common.Models.Config;

public class AdListSourceConfig
{
  [YamlMember(Alias = "url")]
  public string Url { get; set; } = string.Empty;

  [YamlMember(Alias = "type")]
  public string SourceType { get; set; } = string.Empty;

  [YamlMember(Alias = "project_url")]
  public string ProjectUrl { get; set; } = string.Empty;

  [YamlMember(Alias = "maintainer")]
  public string Maintainer { get; set; } = string.Empty;

  [YamlMember(Alias = "enabled")]
  public bool Enabled { get; set; } = true;

  [YamlIgnore]
  public AdListType List { get; set; } = AdListType.Unknown;
}
