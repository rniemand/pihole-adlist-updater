using System.Reflection;
using System.Text.RegularExpressions;
using PiHoleListUpdater.Models;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace PiHoleListUpdater;

static class UpdaterUtils
{
  public static readonly IDeserializer YamlDeserializer = new DeserializerBuilder()
    .WithNamingConvention(UnderscoredNamingConvention.Instance)
    .IgnoreUnmatchedProperties()
    .Build();

  public static string ExeRelative(string path) =>
    Path.Join(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), path);

  public static UpdaterConfig GetConfiguration()
  {
    var exeRelative = UpdaterUtils.ExeRelative("config.yaml");
    if (!File.Exists(exeRelative))
      throw new Exception($"Unable to find configuration file: {exeRelative}");

    var configYaml = File.ReadAllText(exeRelative);
    var config = YamlDeserializer.Deserialize<UpdaterConfig>(configYaml);

    config.Whitelist.CompiledRegex = config.Whitelist.RegexPatterns
      .Select(x => new Regex(x, RegexOptions.Compiled | RegexOptions.Singleline))
      .ToArray();

    return config;
  }
}
