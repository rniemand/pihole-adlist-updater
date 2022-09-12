using PiHoleUpdater.Common.Models;

namespace PiHoleUpdater.Common.Providers;

public interface IBlockListProvider
{
  Task<string> GetBlockListAsync(AdListSourceEntry sourceList);
}

public class BlockListProvider : IBlockListProvider
{
  private readonly HttpClient _httpClient = new();
  
  public async Task<string> GetBlockListAsync(AdListSourceEntry sourceList)
  {
    try
    {
      var request = new HttpRequestMessage(HttpMethod.Get, sourceList.ListUrl);
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
