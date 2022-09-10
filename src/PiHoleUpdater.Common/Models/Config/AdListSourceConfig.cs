using PiHoleUpdater.Common.Enums;
using YamlDotNet.Serialization;

namespace PiHoleUpdater.Common.Models.Config;

public class AdListSourceConfig
{
  [YamlMember(Alias = "url")]
  public string Url { get; set; } = string.Empty;

  [YamlMember(Alias = "enabled")]
  public bool Enabled { get; set; } = true;

  [YamlIgnore]
  public AdList List { get; set; } = AdList.Unknown;
}
