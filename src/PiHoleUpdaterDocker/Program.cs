using PiHoleUpdater.Common.Extensions;
using PiHoleUpdaterDocker;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
      services
        .AddLoggingAndConfig()
        .AddPiHoleUpdater()
        .AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();

// document usage: Newtonsoft.Json
