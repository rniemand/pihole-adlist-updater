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


  // Public
  public async Task WriteCategoryLists(AdList list)
  {
    if (!_config.ListGeneration.GenerateCategoryLists)
      return;

    var entries = (await _domainRepo.GetCompiledListAsync(list))
      .Select(x => x.Domain)
      .ToList();

    var listName = list.ToString("G").ToLower();
    var filePath = Path.Join(_config.Paths.ListsOutput, $"{listName}.txt");
    WriteList(filePath, entries);
  }

  public async Task WriteCombinedLists()
  {
    if (!_config.ListGeneration.GenerateCombinedLists)
      return;

    var entries = (await _domainRepo.GetCompiledListAsync())
      .Select(x => x.Domain)
      .ToList();

    var filePath = Path.Join(_config.Paths.ListsOutput, "_combined.txt");
    WriteList(filePath, entries);
  }


  // Internal
  private void WriteList(string filePath, IEnumerable<string> entries)
  {
    if (!Directory.Exists(_config.Paths.ListsOutput))
      Directory.CreateDirectory(_config.Paths.ListsOutput);

    if (File.Exists(filePath))
      File.Delete(filePath);

    File.WriteAllLines(filePath, entries);
  }
}
