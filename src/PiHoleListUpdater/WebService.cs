namespace PiHoleListUpdater;

internal class WebService
{
  private readonly HttpClient _httpClient = new();
  private readonly bool _usedDevResponses = false;
  private readonly string[] _devResponseFiles = {
    "sample-response-01.txt",
    "sample-response-02.txt",
    "sample-response-03.txt"
  };
  private int _currentResponseIdx = 0;

  public async Task<string> GetUrContentAsync(string url)
  {
    if (_usedDevResponses)
      return GetNextDevResponse();

    Console.WriteLine($"Fetching URL: {url}");
    var request = new HttpRequestMessage(HttpMethod.Get, url);
    var response = await _httpClient.SendAsync(request);
    response.EnsureSuccessStatusCode();
    return await response.Content.ReadAsStringAsync();
  }

  private string GetNextDevResponse()
  {
    var responseFile = _devResponseFiles[_currentResponseIdx++];

    var readAllText = File.ReadAllText($"TestData\\{responseFile}");

    if (_currentResponseIdx + 1 > _devResponseFiles.Length)
      _currentResponseIdx = 0;

    return readAllText;
  }
}