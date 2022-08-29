using System.Reflection;
using PiHoleListUpdater.Models;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace PiHoleListUpdater;

static class Utils
{
  public static readonly IDeserializer YamlDeserializer = new DeserializerBuilder()
    .WithNamingConvention(UnderscoredNamingConvention.Instance)
    .IgnoreUnmatchedProperties()
    .Build();

  public static string ExeRelative(string path) =>
    Path.Join(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), path);

  public static UpdaterConfig GetConfiguration()
  {
    var exeRelative = Utils.ExeRelative("config.yaml");
    if (!File.Exists(exeRelative))
      throw new Exception($"Unable to find configuration file: {exeRelative}");

    var configYaml = File.ReadAllText(exeRelative);
    return YamlDeserializer.Deserialize<UpdaterConfig>(configYaml);
  }
}
