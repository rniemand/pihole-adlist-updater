using YamlDotNet.Serialization;

namespace PiHoleUpdater.Common.Models;

public class GithubCreds
{
  [YamlMember(Alias = "username")]
  public string Username { get; set; } = string.Empty;

  [YamlMember(Alias = "token")]
  public string AccessToken { get; set; } = string.Empty;

  [YamlMember(Alias = "author_name")]
  public string CommitAuthorName { get; set; } = string.Empty;

  [YamlMember(Alias = "author_email")]
  public string CommitAuthorEmail { get; set; } = string.Empty;

  [YamlMember(Alias = "app_name")]
  public string AppName { get; set; } = "PiHoleUpdater";

  [YamlIgnore]
  public bool IsValid
  {
    get
    {
      if (string.IsNullOrWhiteSpace(Username))
        return false;

      return !string.IsNullOrWhiteSpace(AccessToken);
    }
  }
}
