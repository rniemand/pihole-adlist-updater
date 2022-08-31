using PiHoleUpdater.Common.Models.Config;
using PiHoleUpdater.Common.Repo;

namespace PiHoleUpdater.Common.Utils;

public interface IBlockListFileWriter
{
  Task WriteCategoryLists(string category);
  Task WriteCombinedLists();
}

public class BlockListFileWriter : IBlockListFileWriter
{
  private readonly PiHoleUpdaterConfig _config;
  private readonly IDomainRepo _domainRepo;

  public BlockListFileWriter(PiHoleUpdaterConfig config, IDomainRepo domainRepo)
  {
    _config = config;
    _domainRepo = domainRepo;
  }

  public async Task WriteCategoryLists(string category)
  {
    await WriteCategorySafeList(category);
    await WriteCategoryAllList(category);
  }

  public async Task WriteCombinedLists()
  {
    await WriteSafeList();
    await WriteAllList();
  }


  // Internal methods
  private async Task WriteSafeList()
  {
    if (!_config.ListGeneration.CombinedSafe)
      return;

    var entries = (await _domainRepo.GetCompiledListAsync(false))
      .Select(x => x.Domain)
      .ToList();

    var filePath = Path.Join(_config.OutputDir, "_combined-safe.txt");
    WriteList(filePath, entries);
  }

  private async Task WriteCategorySafeList(string category)
  {
    if (!_config.ListGeneration.CategorySafe)
      return;

    var entries = (await _domainRepo.GetCompiledListAsync(category, false))
      .Select(x => x.Domain)
      .ToList();

    var filePath = Path.Join(_config.OutputDir, $"{category}-safe.txt");
    WriteList(filePath, entries);
  }

  private async Task WriteAllList()
  {
    if (!_config.ListGeneration.CombinedAll)
      return;

    var entries = (await _domainRepo.GetCompiledListAsync(true))
      .Select(x => x.Domain)
      .ToList();

    var filePath = Path.Join(_config.OutputDir, "_combined-all.txt");
    WriteList(filePath, entries);
  }

  private async Task WriteCategoryAllList(string category)
  {
    if (!_config.ListGeneration.CategoryAll)
      return;

    var entries = (await _domainRepo.GetCompiledListAsync(category, true))
      .Select(x => x.Domain)
      .ToList();

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
