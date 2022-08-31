using System.Data;
using Dapper;

namespace PiHoleUpdater.Common.Repo;

public class SqlTimeOnlyTypeHandler : SqlMapper.TypeHandler<TimeOnly>
{
  public override void SetValue(IDbDataParameter parameter, TimeOnly time)
  {
    parameter.Value = time.ToString();
  }

  public override TimeOnly Parse(object value)
  {
    return TimeOnly.FromTimeSpan((TimeSpan)value);
  }
}
