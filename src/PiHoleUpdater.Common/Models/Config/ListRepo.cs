using YamlDotNet.Serialization;

namespace PiHoleUpdater.Common.Models.Config;

public class ListRepo
{
  [YamlMember(Alias = "checkout_dir")]
  public string CheckoutDir { get; set; } = string.Empty;

  [YamlMember(Alias = "creds_file")]
  public string CredentialsFile { get; set; } = "./github.creds.yaml";

  [YamlMember(Alias = "enabled")]
  public bool Enabled { get; set; } = true;
}
