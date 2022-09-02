using Microsoft.Extensions.DependencyInjection;
using PiHoleUpdater.Common.Extensions;
using PiHoleUpdater.Common.Services;

ServiceProvider serviceProvider = new ServiceCollection()
  .AddLoggingAndConfig()
  .AddPiHoleUpdater()
  .BuildServiceProvider();

serviceProvider
  .GetRequiredService<IRepoManagerService>()
  .UpdateLocalRepo();

await serviceProvider
  .GetRequiredService<IListUpdaterService>()
  .TickAsync(CancellationToken.None);

serviceProvider
  .GetRequiredService<IRepoManagerService>()
  .CommitChanges();
