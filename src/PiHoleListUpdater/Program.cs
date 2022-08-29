using PiHoleListUpdater;
using PiHoleListUpdater.Models;

UpdaterConfig config = Utils.GetConfiguration();

var webService = new WebService();
var listParser = new AdListParser();
var domains = new HashSet<string>();

foreach (var adList in config.BlockLists)
{
  var rawAdList = await webService.GetUrContentAsync(adList);
  listParser.ParseList(domains, rawAdList);
}

var sortedDomains = domains
  .OrderBy(x => x)
  .ToArray();

if(File.Exists(config.Outputs.Everything))
  File.Delete(config.Outputs.Everything);

File.WriteAllText(config.Outputs.Everything, string.Join("\r\n", domains));

Console.WriteLine(sortedDomains.Length);
