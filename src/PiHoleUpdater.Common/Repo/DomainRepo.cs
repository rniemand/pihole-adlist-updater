using MySql.Data.MySqlClient;
using System.Data;
using Dapper;
using PiHoleUpdater.Common.Enums;
using PiHoleUpdater.Common.Models.Repo;
using PiHoleUpdater.Common.Models.Config;

namespace PiHoleUpdater.Common.Repo;

public interface IDomainRepo
{
  Task<IEnumerable<BlockListEntry>> GetEntriesAsync(AdList list);
  Task<int> AddEntriesAsync(AdList list, IEnumerable<BlockListEntry> entries);
  Task<int> UpdateSeenCountAsync(AdList list, string[] domains);
  Task<IEnumerable<SimpleDomainEntity>> GetCompiledListAsync(AdList list, bool getStrict);
  Task<IEnumerable<SimpleDomainEntity>> GetCompiledListAsync(bool getStrict);
}

public class DomainRepo : IDomainRepo
{
  private readonly MySqlConnection _connection;

  public DomainRepo(PiHoleUpdaterConfig config)
  {
    _connection = new MySqlConnection(config.DbConnectionString);
  }


  // Interface methods
  public async Task<IEnumerable<BlockListEntry>> GetEntriesAsync(AdList list)
  {
    EnsureConnected();

    var query = @$"
    SELECT
      d.`Strict`,
      d.`Domain`,
      {ListQueryHelper.GenerateSelectColumnName(list)}
    FROM `Domains` d
    WHERE
      {ListQueryHelper.GenerateWhereFilter(list, "d")}";

    return await _connection.QueryAsync<BlockListEntry>(query);
  }

  public async Task<int> AddEntriesAsync(AdList list, IEnumerable<BlockListEntry> entries)
  {
    EnsureConnected();

    var currentTime = DateTime.Now.ToString("yyyy-MM-dd");

    var query = @$"
    INSERT INTO `Domains`
      (`DateAdded`, `DateLastSeen`, `Strict`, `Domain`, `{ListQueryHelper.GetColumnName(list)}`)
    VALUES
      ('{currentTime}', '{currentTime}', @Strict, @Domain, 1)";

    return await _connection.ExecuteAsync(query, entries);
  }

  public async Task<int> UpdateSeenCountAsync(AdList list, string[] domains)
  {
    EnsureConnected();

    var query = @$"
    UPDATE `Domains`
    SET
      `SeenCount` = `SeenCount` + 1,
      `DateLastSeen` = curdate(),
      `{ListQueryHelper.GetColumnName(list)}` = 1
    WHERE
      AND `Domain` IN @Domains";

    return await _connection.ExecuteAsync(query, new
    {
      Domains = domains
    });
  }
  
  public async Task<IEnumerable<SimpleDomainEntity>> GetCompiledListAsync(AdList list, bool getStrict)
  {
    EnsureConnected();

    var query = @$"
    SELECT
	    d.`Domain`
    FROM `Domains` d
    WHERE
	    d.`{ListQueryHelper.GetColumnName(list)}` = 1
	    AND d.`Strict` = @Strict
    ORDER BY d.`Domain` ASC";

    return await _connection.QueryAsync<SimpleDomainEntity>(query, new
    {
      Strict = getStrict ? 1 : 0
    });
  }

  public async Task<IEnumerable<SimpleDomainEntity>> GetCompiledListAsync(bool getStrict)
  {
    EnsureConnected();

    const string query = @"
    SELECT
	    d.`Domain`
    FROM `Domains` d
    WHERE
	    d.`Strict` = @Strict
    ORDER BY d.`Domain` ASC";

    return await _connection.QueryAsync<SimpleDomainEntity>(query, new
    {
      Strict = getStrict ? 1 : 0
    });
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
