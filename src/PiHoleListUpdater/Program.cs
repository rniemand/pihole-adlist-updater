using PiHoleListUpdater;
using PiHoleListUpdater.Models;

UpdaterConfig config = Utils.GetConfiguration();

var webService = new WebService(config);
var listParser = new AdListParser();
var domains = new HashSet<string>();

foreach (var adList in config.BlockLists)
{
  Console.WriteLine($"Processing list: {adList.Key}");

  foreach (BlockListConfig listConfig in adList.Value)
  {
    var rawAdList = await webService.GetUrContentAsync(listConfig.ListUrl);
    listParser.ParseList(domains, rawAdList);
  }
}

var sortedDomains = domains
  .OrderBy(x => x)
  .ToArray();

if (File.Exists(config.Outputs.Everything))
  File.Delete(config.Outputs.Everything);

File.WriteAllText(config.Outputs.Everything, string.Join("\r\n", domains));

Console.WriteLine(sortedDomains.Length);
