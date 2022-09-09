using PiHoleUpdater.Common.Logging;

namespace PiHoleUpdater.Common.Providers;

public interface IBlockListProvider
{
  Task<string> GetBlockListAsync(string url);
}

public class BlockListProvider : IBlockListProvider
{
  private readonly ILoggerAdapter<BlockListProvider> _logger;
  private readonly HttpClient _httpClient = new();

  public BlockListProvider(ILoggerAdapter<BlockListProvider> logger)
  {
    _logger = logger;
  }

  public async Task<string> GetBlockListAsync(string url)
  {
    try
    {
      _logger.LogDebug("Fetching URL: {url}", url);
      var request = new HttpRequestMessage(HttpMethod.Get, url);
      var response = await _httpClient.SendAsync(request);
      response.EnsureSuccessStatusCode();
      var rawResponse = await response.Content.ReadAsStringAsync();
      return rawResponse;
    }
    catch (Exception)
    {
      // TODO: (LOGGING) Log this
      return string.Empty;
    }
  }
}
