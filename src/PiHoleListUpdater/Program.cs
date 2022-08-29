using PiHoleListUpdater;
using PiHoleListUpdater.Models;

UpdaterConfig config = Utils.GetConfiguration();

var webService = new WebService(config);
var listParser = new AdListParser();
var compiledBlockLists = new CompiledBlockLists();

foreach (var (listCategory, listEntries) in config.BlockLists)
{
  Console.WriteLine($"Processing list: {listCategory}");
  foreach (BlockListConfig listConfig in listEntries)
  {
    var rawAdList = await webService.GetUrContentAsync(listConfig.ListUrl);
    var entries = listParser.ParseList(rawAdList);
    var addCount = compiledBlockLists.AddEntries(listCategory, entries);
    if(addCount > 0)
      Console.WriteLine($"  > Added {addCount} new entries");
  }
}

Console.WriteLine();
Console.WriteLine("Generating specific lists");
foreach (var listCategory in compiledBlockLists.Categories)
{
  var outListFile = Path.Join(config.OutputDir, $"{listCategory}.txt");


}

Console.WriteLine();
Console.WriteLine();

//var sortedDomains = domains
//  .OrderBy(x => x)
//  .ToArray();

//if (File.Exists(config.Outputs.Everything))
//  File.Delete(config.Outputs.Everything);

//File.WriteAllText(config.Outputs.Everything, string.Join("\r\n", domains));

//Console.WriteLine(sortedDomains.Length);
