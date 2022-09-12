using MySql.Data.MySqlClient;
using PiHoleUpdater.Common.Models.Config;
using PiHoleUpdater.Common.Models.Repo;
using System.Data;
using Dapper;
using PiHoleUpdater.Common.Enums;

namespace PiHoleUpdater.Common.Repo;

public interface IAdListRepo
{
  Task<IEnumerable<AdListEntity>> GetSourceEntries(AdListType listType);
}

public class AdListRepo : IAdListRepo
{
  private readonly MySqlConnection _connection;

  public AdListRepo(PiHoleUpdaterConfig config)
  {
    _connection = new MySqlConnection(config.Database.ConnectionString);
  }


  // Public methods
  public Task<IEnumerable<AdListEntity>> GetSourceEntries(AdListType listType)
  {
    EnsureConnected();

    const string query = @"SELECT *
    FROM `AdLists` al
    WHERE
	    al.`Enabled` = 1
	    AND al.`AdListType` = @listType";

    return _connection.QueryAsync<AdListEntity>(query, new { listType });
  }


  // Internal methods
  private void EnsureConnected()
  {
    // ReSharper disable once SwitchStatementMissingSomeEnumCasesNoDefault
    switch (_connection.State)
    {
      case ConnectionState.Broken:
        _connection.Close();
        _connection.Open();
        return;
      case ConnectionState.Closed:
        _connection.Open();
        break;
    }
  }
}
