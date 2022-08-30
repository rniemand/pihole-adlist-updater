using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using PiHoleUpdater.Common.Logging;
using PiHoleUpdater.Common.Models;
using System.Text.RegularExpressions;
using PiHoleUpdater.Common.Providers;
using PiHoleUpdater.Common.Repo;
using PiHoleUpdater.Common.Services;
using PiHoleUpdater.Common.Utils;
using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.Serialization;

namespace PiHoleUpdater.Common.Extensions;

public static class ServiceCollectionExtensions
{
  private static readonly IDeserializer YamlDeserializer = new DeserializerBuilder()
    .WithNamingConvention(UnderscoredNamingConvention.Instance)
    .IgnoreUnmatchedProperties()
    .Build();

  public static IServiceCollection AddLoggingAndConfig(this IServiceCollection services)
  {
    IConfigurationRoot? config = new ConfigurationBuilder()
      .AddJsonFile("appsettings.json", optional: true)
      .Build();

    return services
      .AddLogging(loggingBuilder =>
      {
        // configure Logging with NLog
        loggingBuilder.ClearProviders();
        loggingBuilder.SetMinimumLevel(LogLevel.Trace);
        loggingBuilder.AddNLog(config);
      })
      .AddSingleton(GetConfiguration())
      .AddSingleton(typeof(ILoggerAdapter<>), typeof(LoggerAdapter<>))
      .AddSingleton<IConfiguration>(config);
  }

  public static IServiceCollection AddPiHoleUpdater(this IServiceCollection services)
  {
    return services
      .AddSingleton<IDomainRepo, DomainRepo>()
      .AddSingleton<IBlockListEntryParser, BlockListEntryParser>()
      .AddSingleton<IBlockListFileWriter, BlockListFileWriter>()
      .AddSingleton<IBlockListWebProvider, BlockListWebProvider>()
      .AddSingleton<IListUpdaterService, ListUpdaterService>()
      .AddSingleton<IDomainTrackerService, DomainTrackerService>();
  }


  // Internal methods
  private static UpdaterConfig GetConfiguration()
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
