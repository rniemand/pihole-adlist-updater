using PiHoleUpdater.Common.Models;
using PiHoleUpdater.Common.Models.Config;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace PiHoleUpdater.Common.Providers;

public interface IGithubCredsProvider
{
  GithubCreds GetCredentials();
}

public class GithubCredsProvider : IGithubCredsProvider
{
  private readonly GithubCreds _credentials;

  public GithubCredsProvider(PiHoleUpdaterConfig config)
  {
    _credentials = LoadCredentials(config);
  }

  public GithubCreds GetCredentials() => _credentials;

  private static GithubCreds LoadCredentials(PiHoleUpdaterConfig config)
  {
    var filePath = config.Repo.CredentialsFile;
    if (filePath.StartsWith("./"))
      filePath = UpdaterUtils.ExeRelative(filePath.Replace("./", ""));

    if (!File.Exists(filePath))
      throw new Exception("Unable to find: github.creds.yaml");

    var yamlDeserializer = new DeserializerBuilder()
      .WithNamingConvention(UnderscoredNamingConvention.Instance)
      .IgnoreUnmatchedProperties()
      .Build();

    return yamlDeserializer.Deserialize<GithubCreds>(File.ReadAllText(filePath));
  }
}
