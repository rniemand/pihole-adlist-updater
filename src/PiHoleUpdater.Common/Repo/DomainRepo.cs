using MySql.Data.MySqlClient;
using PiHoleUpdater.Common.Logging;
using PiHoleUpdater.Common.Models;
using System.Data;
using Dapper;

namespace PiHoleUpdater.Common.Repo;

public interface IDomainRepo
{
  Task<IEnumerable<BlockListEntry>> GetEntriesAsync(string listName);
  Task<int> AddEntriesAsync(IEnumerable<BlockListEntry> entries);
  Task<int> UpdateSeenCountAsync(string listName, string[] domains);
  Task<int> DeleteEntriesAsync(string listName, string[] domains);
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
  public async Task<IEnumerable<BlockListEntry>> GetEntriesAsync(string listName)
  {
    ensureConnected();

    const string query = @"
    SELECT
      d.`Restrictive`,
      d.`Domain`,
      d.`ListName`
    FROM `Domains` d
    WHERE d.`ListName` = @ListName
      AND d.`Deleted` = 0";

    return await _connection.QueryAsync<BlockListEntry>(query, new { ListName = listName });
  }

  public async Task<int> AddEntriesAsync(IEnumerable<BlockListEntry> entries)
  {
    ensureConnected();

    const string query = @"
    INSERT INTO `Domains`
      (`Restrictive`, `Domain`, `ListName`)
    VALUES
      (@Restrictive, @Domain, @ListName)";

    return await _connection.ExecuteAsync(query, entries);
  }

  public async Task<int> UpdateSeenCountAsync(string listName, string[] domains)
  {
    ensureConnected();

    const string query = @"
    UPDATE `Domains`
    SET
      `SeenCount` = `SeenCount` + 1,
      `DateLastSeen` = curdate()
    WHERE
      `ListName` = @ListName
      AND `Domain` IN @Domains";

    return await _connection.ExecuteAsync(query, new
    {
      ListName = listName,
      Domains = domains
    });
  }

  public async Task<int> DeleteEntriesAsync(string listName, string[] domains)
  {
    ensureConnected();

    const string query = @"
    UPDATE `Domains`
    SET
      `Deleted` = 1
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
