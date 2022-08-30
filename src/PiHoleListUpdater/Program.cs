using PiHoleListUpdater;
using PiHoleListUpdater.Models;

var config = UpdaterUtils.GetConfiguration();
var webService = new BlockListWebService(config);
var listParser = new BlockListEntryParser(config);
var listDumper = new BlockListFileWriter(config);
var blockLists = new CompiledBlockLists();

Console.WriteLine("=============================================");
Console.WriteLine("Processing lists...");
Console.WriteLine("=============================================");
foreach (var (listCategory, listEntries) in config.BlockLists)
{
  Console.WriteLine($"Processing list: {listCategory}");
  foreach (BlockListConfig listConfig in listEntries)
  {
    var rawAdList = await webService.GetUrContentAsync(listConfig.ListUrl);
    var entries = listParser.ParseList(rawAdList);
    var addCount = blockLists.AddDomains(listCategory, entries, listConfig.Restrictive);
    if(addCount > 0)
      Console.WriteLine($"  > Added {addCount} new entries");
  }
}

if (config.ListGeneration.GenerateCategoryLists)
{
  Console.WriteLine();
  Console.WriteLine("=============================================");
  Console.WriteLine("Generating category lists...");
  Console.WriteLine("=============================================");
  foreach (var listCategory in blockLists.Categories)
    listDumper.WriteCategoryLists(listCategory, blockLists);
}

if (config.ListGeneration.GenerateCombinedLists)
{
  Console.WriteLine();
  Console.WriteLine("=============================================");
  Console.WriteLine("Generating combined lists...");
  Console.WriteLine("=============================================");
  listDumper.WriteCombinedLists(blockLists);
}



Console.WriteLine();
Console.WriteLine();
