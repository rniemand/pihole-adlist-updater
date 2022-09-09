using PiHoleUpdater.Common.Enums;
using PiHoleUpdater.Common.Models.Config;
using PiHoleUpdater.Common.Repo;

namespace PiHoleUpdater.Common.Utils;

public interface IBlockListFileWriter
{
  Task WriteCategoryLists(AdList list);
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

  public async Task WriteCategoryLists(AdList list)
  {
    await WriteCategoryList(list);
    await WriteCategoryStrictList(list);
  }

  public async Task WriteCombinedLists()
  {
    await WriteList();
    await WriteStrictList();
  }


  // Internal methods
  private async Task WriteList()
  {
    if (!_config.ListGeneration.CombinedSafe)
      return;

    var entries = (await _domainRepo.GetCompiledListAsync(false))
      .Select(x => x.Domain)
      .ToList();

    var filePath = Path.Join(_config.OutputDir, "_combined.txt");
    WriteList(filePath, entries);
  }

  private async Task WriteCategoryList(AdList list)
  {
    if (!_config.ListGeneration.CategorySafe)
      return;

    var entries = (await _domainRepo.GetCompiledListAsync(list, false))
      .Select(x => x.Domain)
      .ToList();

    var listName = list.ToString("G").ToLower();
    var filePath = Path.Join(_config.OutputDir, $"{listName}.txt");
    WriteList(filePath, entries);
  }

  private async Task WriteStrictList()
  {
    if (!_config.ListGeneration.CombinedAll)
      return;

    var entries = (await _domainRepo.GetCompiledListAsync(true))
      .Select(x => x.Domain)
      .ToList();

    var filePath = Path.Join(_config.OutputDir, "_combined-strict.txt");
    WriteList(filePath, entries);
  }

  private async Task WriteCategoryStrictList(AdList list)
  {
    if (!_config.ListGeneration.CategoryAll)
      return;

    var entries = (await _domainRepo.GetCompiledListAsync(list, true))
      .Select(x => x.Domain)
      .ToList();

    var listName = list.ToString("G").ToLower();
    var filePath = Path.Join(_config.OutputDir, $"{listName}-strict.txt");
    WriteList(filePath, entries);
  }

  private void WriteList(string filePath, IReadOnlyCollection<string> entries)
  {
    if (!Directory.Exists(_config.OutputDir))
      Directory.CreateDirectory(_config.OutputDir);

    if (File.Exists(filePath))
      File.Delete(filePath);

    File.WriteAllLines(filePath, entries);
  }
}
