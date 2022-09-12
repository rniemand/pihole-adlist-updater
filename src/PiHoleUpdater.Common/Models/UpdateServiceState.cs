using Newtonsoft.Json;

namespace PiHoleUpdater.Common.Models;

public class UpdateServiceState
{
  [JsonProperty("nextTick")]
  public DateTime NextTick { get; set; } = DateTime.Now;

  [JsonProperty("lastTick")]
  public DateTime LastTick { get; set; }=DateTime.MinValue;
}
