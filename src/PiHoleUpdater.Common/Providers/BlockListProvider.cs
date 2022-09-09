using PiHoleUpdater.Common.Logging;
using PiHoleUpdater.Common.Models.Config;

namespace PiHoleUpdater.Common.Providers;

public interface IBlockListProvider
{
  Task<string> GetBlockListAsync(string url);
}

public class BlockListProvider : IBlockListProvider
{
  private readonly ILoggerAdapter<BlockListProvider> _logger;
  private readonly HttpClient _httpClient = new();
  private readonly string[] _devResponseFiles;
  private readonly bool _usedDevResponses;
  private readonly bool _captureResponses;
  private readonly string _captureResponseDir;
  private int _currentResponseIdx;
  private int _captureResponseNumber = 1;

  public BlockListProvider(ILoggerAdapter<BlockListProvider> logger, PiHoleUpdaterConfig config)
  {
    _logger = logger;
    _captureResponses = config.Development.CaptureResponses;
    _usedDevResponses = config.Development.Enabled && config.Development.UseCachedLists;
    _devResponseFiles = GetDevResponseFiles(config);
    _captureResponseDir = config.Development.CaptureResponseDir;

    if (_captureResponses && string.IsNullOrWhiteSpace(_captureResponseDir))
      throw new Exception("No capture response directory defined");
  }

  public async Task<string> GetBlockListAsync(string url)
  {
    if (_usedDevResponses)
      return GetNextDevResponse();

    try
    {
      _logger.LogDebug("Fetching URL: {url}", url);
      var request = new HttpRequestMessage(HttpMethod.Get, url);
      var response = await _httpClient.SendAsync(request);
      response.EnsureSuccessStatusCode();
      var rawResponse = await response.Content.ReadAsStringAsync();
      CaptureResponse(rawResponse);
      return rawResponse;
    }
    catch (Exception)
    {
      // TODO: (LOGGING) Log this
      return string.Empty;
    }
  }

  private string GetNextDevResponse()
  {
    var responseFile = _devResponseFiles[_currentResponseIdx++];
    var readAllText = File.ReadAllText(responseFile);

    if (_currentResponseIdx + 1 > _devResponseFiles.Length)
      _currentResponseIdx = 0;

    return readAllText;
  }


  // Internal methods
  private string[] GetDevResponseFiles(PiHoleUpdaterConfig config)
  {
    if (!_usedDevResponses)
      return Array.Empty<string>();

    if (string.IsNullOrWhiteSpace(config.Development.CachedResponseDir))
      config.Development.CachedResponseDir = "TestData/";

    var files = new DirectoryInfo(config.Development.CachedResponseDir)
      .GetFiles("*.txt", SearchOption.TopDirectoryOnly)
      .Select(x => x.FullName)
      .ToArray();

    if (files.Length == 0)
      throw new Exception($"Unable to find any dev response files in: {config.Development.CachedResponseDir}");

    return files;
  }

  private void CaptureResponse(string rawResponse)
  {
    if (!_captureResponses)
      return;

    var captureNumber = (_captureResponseNumber++).ToString("D").PadLeft(3, '0');
    var fileName = Path.Join(_captureResponseDir, $"captured-response-{captureNumber}.txt");

    if (!Directory.Exists(_captureResponseDir))
      Directory.CreateDirectory(_captureResponseDir);

    if (File.Exists(fileName))
      File.Delete(fileName);

    Console.WriteLine($"Dumping captured response to: {fileName}");
    File.WriteAllText(fileName, rawResponse);
  }
}
