using MySql.Data.MySqlClient;
using PiHoleUpdater.Common.Models.Config;
using PiHoleUpdater.Common.Models.Repo;
using System.Data;
using Dapper;

namespace PiHoleUpdater.Common.Repo;

public interface IWhitelistRepo
{
  Task<IEnumerable<WhitelistEntity>> GetEntriesAsync();
}

public class WhitelistRepo : IWhitelistRepo
{
  private readonly MySqlConnection _connection;

  public WhitelistRepo(PiHoleUpdaterConfig config)
  {
    _connection = new MySqlConnection(config.Database.ConnectionString);
  }

  public Task<IEnumerable<WhitelistEntity>> GetEntriesAsync()
  {
    const string query = @"SELECT *
    FROM `Whitelists` wl
    WHERE
	    wl.`Enabled` = 1
    ORDER BY
	    wl.`IsRegex`, wl.`Order` ASC";

    EnsureConnected();

    return _connection.QueryAsync<WhitelistEntity>(query);
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
