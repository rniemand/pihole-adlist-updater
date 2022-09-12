namespace PiHoleUpdater.Common.Models;

public class UpdateServiceState
{
  public DateTime NextTick { get; set; } = DateTime.Now;
  public DateTime LastTick { get; set; }=DateTime.MinValue;
}
