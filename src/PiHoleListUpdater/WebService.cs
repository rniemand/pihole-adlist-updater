using PiHoleListUpdater.Models;

namespace PiHoleListUpdater;

internal class WebService
{
  private readonly HttpClient _httpClient = new();
  private readonly bool _usedDevResponses;
  private readonly string[] _devResponseFiles;
  private int _currentResponseIdx;

  public WebService(UpdaterConfig config)
  {
    _usedDevResponses = config.Development.Enabled && config.Development.UseCachedLists;
    _devResponseFiles = GetDevResponseFiles(config);
  }

  public async Task<string> GetUrContentAsync(string url)
  {
    if (_usedDevResponses)
      return GetNextDevResponse();

    try
    {
      Console.WriteLine($"Fetching URL: {url}");
      var request = new HttpRequestMessage(HttpMethod.Get, url);
      HttpResponseMessage response = await _httpClient.SendAsync(request);
      response.EnsureSuccessStatusCode();
      return await response.Content.ReadAsStringAsync();
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
  private string[] GetDevResponseFiles(UpdaterConfig config)
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
}
