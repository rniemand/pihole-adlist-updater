using MySql.Data.MySqlClient;
using PiHoleUpdater.Common.Logging;
using PiHoleUpdater.Common.Models;
using System.Data;
using Dapper;

namespace PiHoleUpdater.Common.Repo;

public interface IDomainRepo
{
  Task<IEnumerable<DomainEntity>> GetListEntries(string listName);
}

public class DomainRepo : IDomainRepo
{
  private readonly ILoggerAdapter<DomainRepo> _logger;
  private readonly MySqlConnection _connection;

  public DomainRepo(ILoggerAdapter<DomainRepo> logger, UpdaterConfig config)
  {
    _logger = logger;
    _connection = new MySqlConnection(config.DbConnectionString);
  }


  // Interface methods
  public async Task<IEnumerable<DomainEntity>> GetListEntries(string listName)
  {
    const string query = @"
    SELECT *
    FROM `Domains` d
    WHERE d.`ListName` = @ListName";

    ensureConnected();

    return await _connection.QueryAsync<DomainEntity>(query, new { ListName = listName });
  }


  // Internal methods
  private void ensureConnected()
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
