using PiHoleListUpdater.Models;

namespace PiHoleListUpdater;

class BlockListDumper
{
  private readonly UpdaterConfig _config;

  public BlockListDumper(UpdaterConfig config)
  {
    _config = config;
  }

  public void DumpList(string category, CompiledBlockLists lists)
  {
    Console.WriteLine($"  Dumping '{category}' list");
    DumpCategorySafeList(category, lists);
    DumpCategoryAllList(category, lists);
  }


  // Internal methods
  private void DumpCategorySafeList(string category, CompiledBlockLists lists)
  {
    if(!_config.ListGeneration.CategorySafe)
      return;

    var entries = lists.GetListEntries(category);
    var filePath = Path.Join(_config.OutputDir, $"{category}-safe.txt");
    WriteList(filePath, entries);
  }

  private void DumpCategoryAllList(string category, CompiledBlockLists lists)
  {
    if (!_config.ListGeneration.CategoryAll)
      return;

    var entries = lists.GetAllListEntries(category);
    var filePath = Path.Join(_config.OutputDir, $"{category}-all.txt");
    WriteList(filePath, entries);
  }

  private void WriteList(string filePath, IReadOnlyCollection<string> entries)
  {
    Console.WriteLine($"   - writing {entries.Count} to {filePath}");

    if (!Directory.Exists(_config.OutputDir))
      Directory.CreateDirectory(_config.OutputDir);

    if (File.Exists(filePath))
      File.Delete(filePath);

    File.WriteAllLines(filePath, entries);
  }
}
