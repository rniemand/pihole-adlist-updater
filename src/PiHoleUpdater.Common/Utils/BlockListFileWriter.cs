using System.Text;
using PiHoleUpdater.Common.Enums;
using PiHoleUpdater.Common.Models;
using PiHoleUpdater.Common.Models.Config;
using PiHoleUpdater.Common.Repo;

namespace PiHoleUpdater.Common.Utils;

public interface IBlockListFileWriter
{
  Task WriteCategoryLists(AdListType listType, List<AdListSourceEntry> sources);
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
  public async Task WriteCategoryLists(AdListType listType, List<AdListSourceEntry> sources)
  {
    if (!_config.ListGeneration.GenerateCategoryLists)
      return;

    var entries = (await _domainRepo.GetCompiledListAsync(listType))
      .Select(x => x.Domain)
      .ToList();

    var listContents = GenerateAdListContent(sources, entries);
    var listName = listType.ToString("G").ToLower();
    var filePath = Path.Join(_config.ListGeneration.OutputDir, $"{listName}.txt");
    WriteList(filePath, listContents);
  }

  public async Task WriteCombinedLists()
  {
    if (!_config.ListGeneration.GenerateCombinedLists)
      return;

    var entries = (await _domainRepo.GetCompiledListAsync())
      .Select(x => x.Domain)
      .ToList();

    var filePath = Path.Join(_config.ListGeneration.OutputDir, "_combined.txt");
    WriteList(filePath, GenerateCombinedList(entries));
  }


  // Internal
  private void WriteList(string filePath, string contents)
  {
    if (!Directory.Exists(_config.ListGeneration.OutputDir))
      Directory.CreateDirectory(_config.ListGeneration.OutputDir);

    if (File.Exists(filePath))
      File.Delete(filePath);

    File.WriteAllText(filePath, contents);
  }

  private static string GenerateAdListContent(List<AdListSourceEntry> sources, List<string> domains)
  {
    var builder = new StringBuilder(GenerateListHeader(sources));

    foreach (var domain in domains)
      builder.AppendLine(domain);

    return builder.ToString();
  }

  private static string GenerateListHeader(List<AdListSourceEntry> sources)
  {
    var builder = new StringBuilder()
      .AppendLine("# ==============================================")
      .AppendLine($"# AdList generated on: {DateTime.Now.ToString("R")}")
      .AppendLine($"# Project URL: {AppConstants.ProjectUrl}")
      .AppendLine("# ==============================================")
      .AppendLine($"# Generated from {sources.Count} source(s)")
      .AppendLine("# ");

    foreach (var source in sources)
      builder.AppendLine($"# {source.SourceType} | {source.Maintainer} | {source.ProjectUrl}");

    builder
      .AppendLine("# ")
      .AppendLine("# ==============================================");

    return builder.ToString();
  }

  private static string GenerateCombinedList(List<string> domains)
  {
    var builder = new StringBuilder()
      .AppendLine("# ==============================================")
      .AppendLine($"# AdList generated on: {DateTime.Now.ToString("R")}")
      .AppendLine($"# Project URL: {AppConstants.ProjectUrl}")
      .AppendLine("# ==============================================")
      .AppendLine("# ");

    foreach (var domain in domains)
      builder.AppendLine(domain);

    return builder.ToString();
  }
}
