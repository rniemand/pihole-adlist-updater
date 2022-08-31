using MySql.Data.MySqlClient;
using PiHoleUpdater.Common.Logging;
using PiHoleUpdater.Common.Models;
using System.Data;
using Dapper;

namespace PiHoleUpdater.Common.Repo;

public interface IDomainRepo
{
  Task<IEnumerable<BlockListEntry>> GetListEntries(string listName);
  Task<int> AddNewEntries(IEnumerable<BlockListEntry> entries);
  Task<int> UpdateSeen(string listName, string[] domains);
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
  public async Task<IEnumerable<BlockListEntry>> GetListEntries(string listName)
  {
    ensureConnected();

    const string query = @"
    SELECT
      d.`Restrictive`,
      d.`Domain`,
      d.`ListName`
    FROM `Domains` d
    WHERE d.`ListName` = @ListName";

    return await _connection.QueryAsync<BlockListEntry>(query, new { ListName = listName });
  }

  public async Task<int> AddNewEntries(IEnumerable<BlockListEntry> entries)
  {
    ensureConnected();

    const string query = @"
    INSERT INTO `Domains`
      (`Restrictive`, `Domain`, `ListName`)
    VALUES
      (@Restrictive, @Domain, @ListName)";

    return await _connection.ExecuteAsync(query, entries);
  }

  public async Task<int> UpdateSeen(string listName, string[] domains)
  {
    ensureConnected();

    const string query = @"
    UPDATE `Domains`
    SET
      `SeenCount` = `SeenCount` + 1
    WHERE
      `ListName` = @ListName
      AND `Domain` IN @Domains";

    return await _connection.ExecuteAsync(query, new
    {
      ListName = listName,
      Domains = domains
    });
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
