using PiHoleListUpdater;
using PiHoleListUpdater.Models;

UpdaterConfig config = Utils.GetConfiguration();

var webService = new WebService(config);
var listParser = new BlockListParser(config);
var listDumper = new BlockListDumper(config);
var compiledBlockLists = new CompiledBlockLists();

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
    var addCount = compiledBlockLists.AddEntries(listCategory, entries, listConfig.Restrictive);
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
  foreach (var listCategory in compiledBlockLists.Categories)
    listDumper.DumpCategoryList(listCategory, compiledBlockLists);
}

if (config.ListGeneration.GenerateCombinedLists)
{
  Console.WriteLine();
  Console.WriteLine("=============================================");
  Console.WriteLine("Generating combined lists...");
  Console.WriteLine("=============================================");
  listDumper.DumpList(compiledBlockLists);
}



Console.WriteLine();
Console.WriteLine();
