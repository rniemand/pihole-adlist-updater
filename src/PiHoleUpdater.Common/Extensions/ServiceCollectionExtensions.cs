using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using PiHoleUpdater.Common.Logging;
using System.Text.RegularExpressions;
using PiHoleUpdater.Common.Providers;
using PiHoleUpdater.Common.Repo;
using PiHoleUpdater.Common.Services;
using PiHoleUpdater.Common.Utils;
using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.Serialization;
using Dapper;
using PiHoleUpdater.Common.Models.Config;

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
    SqlMapper.AddTypeHandler(new SqlTimeOnlyTypeHandler());
    SqlMapper.AddTypeHandler(new DapperSqlDateOnlyTypeHandler());

    return services
      .AddSingleton<IDomainRepo, DomainRepo>()
      .AddSingleton<IBlockListParser, BlockListParser>()
      .AddSingleton<IBlockListFileWriter, BlockListFileWriter>()
      .AddSingleton<IBlockListProvider, BlockListProvider>()
      .AddSingleton<IListUpdaterService, ListUpdaterService>()
      .AddSingleton<IDomainTrackingService, DomainTrackingService>();
  }


  // Internal methods
  private static PiHoleUpdaterConfig GetConfiguration()
  {
    var exeRelative = UpdaterUtils.ExeRelative("config.yaml");
    if (!File.Exists(exeRelative))
      throw new Exception($"Unable to find configuration file: {exeRelative}");

    var configYaml = File.ReadAllText(exeRelative);
    var config = YamlDeserializer.Deserialize<PiHoleUpdaterConfig>(configYaml);

    config.Whitelist.CompiledRegex = config.Whitelist.RegexPatterns
      .Select(x => new Regex(x, RegexOptions.Compiled | RegexOptions.Singleline))
      .ToArray();

    return config;
  }
}
