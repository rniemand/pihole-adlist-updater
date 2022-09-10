using Microsoft.Extensions.DependencyInjection;
using PiHoleUpdater.Common.Extensions;
using PiHoleUpdater.Common.Models.Config;
using PiHoleUpdater.Common.Services;

var serviceProvider = new ServiceCollection()
  .AddLoggingAndConfig()
  .AddPiHoleUpdater()
  .BuildServiceProvider();

var config = serviceProvider.GetRequiredService<PiHoleUpdaterConfig>();
var repoService = serviceProvider.GetRequiredService<IRepoManagerService>();

if (config.Repo.Enabled)
  repoService.UpdateLocalRepo();

await serviceProvider
  .GetRequiredService<IAdListService>()
  .TickAsync(CancellationToken.None);

if (config.Repo.Enabled)
  repoService.CommitChanges();
