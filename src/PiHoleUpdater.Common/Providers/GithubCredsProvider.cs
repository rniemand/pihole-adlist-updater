using PiHoleUpdater.Common.Logging;
using PiHoleUpdater.Common.Models;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace PiHoleUpdater.Common.Providers;

public interface IGithubCredsProvider
{
  GithubCreds GetCredentials();
}

public class GithubCredsProvider : IGithubCredsProvider
{
  private readonly ILoggerAdapter<GithubCredsProvider> _logger;

  public GithubCredsProvider(ILoggerAdapter<GithubCredsProvider> logger)
  {
    _logger = logger;
  }

  public GithubCreds GetCredentials()
  {
    var filePath = UpdaterUtils.ExeRelative("github.creds.yaml");

    if (!File.Exists(filePath))
      throw new Exception("Unable to find: github.creds.yaml");

    IDeserializer YamlDeserializer = new DeserializerBuilder()
      .WithNamingConvention(UnderscoredNamingConvention.Instance)
      .IgnoreUnmatchedProperties()
      .Build();

    return YamlDeserializer.Deserialize<GithubCreds>(File.ReadAllText(filePath));
  }
}
