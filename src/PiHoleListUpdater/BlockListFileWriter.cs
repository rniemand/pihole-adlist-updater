using PiHoleListUpdater.Models;

namespace PiHoleListUpdater;

class BlockListFileWriter
{
  private readonly UpdaterConfig _config;

  public BlockListFileWriter(UpdaterConfig config)
  {
    _config = config;
  }

  public void WriteCategoryLists(string category, CompiledBlockLists lists)
  {
    Console.WriteLine($"  Dumping '{category}' list");
    WriteCategorySafeList(category, lists);
    WriteCategoryAllList(category, lists);
  }

  public void WriteCombinedLists(CompiledBlockLists lists)
  {
    WriteSafeList(lists);
    WriteAllList(lists);
  }


  // Internal methods
  private void WriteSafeList(CompiledBlockLists lists)
  {
    if (!_config.ListGeneration.CategorySafe)
      return;

    var entries = lists.GetSafeEntries();
    var filePath = Path.Join(_config.OutputDir, "_combined-safe.txt");
    WriteList(filePath, entries);
  }

  private void WriteCategorySafeList(string category, CompiledBlockLists lists)
  {
    if(!_config.ListGeneration.CategorySafe)
      return;

    var entries = lists.GetSafeEntries(category);
    var filePath = Path.Join(_config.OutputDir, $"{category}-safe.txt");
    WriteList(filePath, entries);
  }

  private void WriteAllList(CompiledBlockLists lists)
  {
    if (!_config.ListGeneration.CategorySafe)
      return;

    var entries = lists.GetAllEntries();
    var filePath = Path.Join(_config.OutputDir, "_combined-all.txt");
    WriteList(filePath, entries);
  }

  private void WriteCategoryAllList(string category, CompiledBlockLists lists)
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
