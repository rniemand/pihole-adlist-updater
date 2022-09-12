using Microsoft.Extensions.DependencyInjection;
using PiHoleUpdater.Common.Extensions;
using PiHoleUpdater.Common.Models.Config;
using PiHoleUpdater.Common.Services;

ServiceProvider serviceProvider = new ServiceCollection()
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

/*
 * TODO:
 *   - Add support for extracting portions out of a specific block list
 *   - Add support for these values | ":: 0-checksrv.net.daraz.com"
 *   - port additional lists from here | https://obutterbach.medium.com/unlock-the-full-potential-of-pihole-e795342e0e36
 *   - ADD THIS: https://trainax.github.io/PiHoleLists/
 *
 * LISTS:
 *   - https://gitlab.com/quidsup/notrack-blocklists/raw/master/notrack-blocklist.txt
 *   - https://raw.githubusercontent.com/notracking/hosts-blocklists/master/hostnames.txt
 *   - https://codeberg.org/spootle/blocklist
 *   - https://pcriver.com/uncategorized/best-pihole-block-lists-how-to-update-pi-hole-2.html
 *
 */
