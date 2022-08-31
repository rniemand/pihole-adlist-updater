namespace PiHoleUpdater.Common.Abstractions;

public interface IDateTimeAbstraction
{
  DateTime Now { get; }
}

public class DateTimeAbstraction : IDateTimeAbstraction
{
  public DateTime Now => DateTime.Now;
}
