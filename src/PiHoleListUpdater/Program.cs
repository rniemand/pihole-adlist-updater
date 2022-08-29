using PiHoleListUpdater;
using PiHoleListUpdater.Models;

UpdaterConfig updaterConfig = Utils.GetConfiguration();


var webService = new WebService();
var listParser = new AdListParser();
var domains = new HashSet<string>();
const string outFilePath = "C:\\WRK\\pihole-adlist\\lists\\everything.txt";

foreach (var adList in updaterConfig.BlockLists)
{
  var rawAdList = await webService.GetUrContentAsync(adList);
  listParser.ParseList(domains, rawAdList);
}

var sortedDomains = domains
  .OrderBy(x => x)
  .ToArray();

if(File.Exists(outFilePath))
  File.Delete(outFilePath);

File.WriteAllText(outFilePath, string.Join("\r\n", domains));

Console.WriteLine(sortedDomains.Length);
