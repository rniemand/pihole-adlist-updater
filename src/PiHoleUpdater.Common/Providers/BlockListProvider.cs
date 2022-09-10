using PiHoleUpdater.Common.Logging;
using PiHoleUpdater.Common.Models.Config;

namespace PiHoleUpdater.Common.Providers;

public interface IBlockListProvider
{
  Task<string> GetBlockListAsync(AdListSourceConfig sourceList);
}

public class BlockListProvider : IBlockListProvider
{
  private readonly ILoggerAdapter<BlockListProvider> _logger;
  private readonly HttpClient _httpClient = new();

  public BlockListProvider(ILoggerAdapter<BlockListProvider> logger)
  {
    _logger = logger;
  }

  public async Task<string> GetBlockListAsync(AdListSourceConfig sourceList)
  {
    try
    {
      _logger.LogDebug("Fetching list: {list}", sourceList.List);
      var request = new HttpRequestMessage(HttpMethod.Get, sourceList.Url);
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
