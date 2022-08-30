using PiHoleListUpdater;
using PiHoleListUpdater.Models;

var config = UpdaterUtils.GetConfiguration();
var webService = new BlockListWebService(config);
var listParser = new BlockListEntryParser(config);
var listDumper = new BlockListFileWriter(config);
var blockLists = new CompiledBlockLists();


UpdaterUtils.WriteHeading("Processing lists...");
foreach (var (listCategory, listEntries) in config.BlockLists)
{
  Console.WriteLine($"Processing list: {listCategory}");
  foreach (var listConfig in listEntries)
  {
    var rawBlockList = await webService.GetBlockListAsync(listConfig.ListUrl);
    var addCount = blockLists.AddDomains(listCategory,
      listParser.ParseList(rawBlockList),
      listConfig.Restrictive);

    if(addCount > 0)
      Console.WriteLine($"  > Added {addCount} new entries");
  }
}

if (config.ListGeneration.GenerateCategoryLists)
{
  UpdaterUtils.WriteHeading("Generating category lists...");
  foreach (var listCategory in blockLists.Categories)
    listDumper.WriteCategoryLists(listCategory, blockLists);
}

if (config.ListGeneration.GenerateCombinedLists)
{
  UpdaterUtils.WriteHeading("Generating combined lists...");
  listDumper.WriteCombinedLists(blockLists);
}

UpdaterUtils.WriteHeading("All done.");
