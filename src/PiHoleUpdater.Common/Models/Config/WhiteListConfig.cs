using System.Text.RegularExpressions;
using YamlDotNet.Serialization;

namespace PiHoleUpdater.Common.Models.Config;

public class WhiteListConfig
{
  [YamlMember(Alias = "regex")]
  public string[] RegexPatterns { get; set; } = Array.Empty<string>();

  [YamlIgnore]
  public Regex[] CompiledRegex { get; set; } = Array.Empty<Regex>();

  [YamlMember(Alias = "exact")]
  public string[] ExactDomains { get; set; } = Array.Empty<string>();
}
