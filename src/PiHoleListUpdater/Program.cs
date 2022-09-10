using Microsoft.Extensions.DependencyInjection;
using PiHoleUpdater.Common.Extensions;
using PiHoleUpdater.Common.Services;

ServiceProvider serviceProvider = new ServiceCollection()
  .AddLoggingAndConfig()
  .AddPiHoleUpdater()
  .BuildServiceProvider();

//serviceProvider
//  .GetRequiredService<IRepoManagerService>()
//  .UpdateLocalRepo();

await serviceProvider
  .GetRequiredService<IAdListService>()
  .TickAsync(CancellationToken.None);

//serviceProvider
//  .GetRequiredService<IRepoManagerService>()
//  .CommitChanges();
