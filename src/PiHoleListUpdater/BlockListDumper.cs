using PiHoleListUpdater.Models;

namespace PiHoleListUpdater;

class BlockListDumper
{
  private readonly UpdaterConfig _config;

  public BlockListDumper(UpdaterConfig config)
  {
    _config = config;
  }

  public void DumpCategoryList(string category, CompiledBlockLists lists)
  {
    Console.WriteLine($"  Dumping '{category}' list");
    DumpCategorySafeList(category, lists);
    DumpCategoryAllList(category, lists);
  }

  public void DumpList(CompiledBlockLists lists)
  {
    DumpSafeList(lists);
    DumpAllList(lists);
  }


  // Internal methods
  private void DumpSafeList(CompiledBlockLists lists)
  {
    if (!_config.ListGeneration.CategorySafe)
      return;

    var entries = lists.GetSafeEntries();
    var filePath = Path.Join(_config.OutputDir, "_combined-safe.txt");
    WriteList(filePath, entries);
  }

  private void DumpCategorySafeList(string category, CompiledBlockLists lists)
  {
    if(!_config.ListGeneration.CategorySafe)
      return;

    var entries = lists.GetSafeEntries(category);
    var filePath = Path.Join(_config.OutputDir, $"{category}-safe.txt");
    WriteList(filePath, entries);
  }

  private void DumpAllList(CompiledBlockLists lists)
  {
    if (!_config.ListGeneration.CategorySafe)
      return;

    var entries = lists.GetAllEntries();
    var filePath = Path.Join(_config.OutputDir, "_combined-all.txt");
    WriteList(filePath, entries);
  }

  private void DumpCategoryAllList(string category, CompiledBlockLists lists)
  {
    if (!_config.ListGeneration.CategoryAll)
      return;

    var entries = lists.GetAllEntries(category);
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
