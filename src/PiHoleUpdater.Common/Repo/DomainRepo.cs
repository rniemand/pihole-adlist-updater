using MySql.Data.MySqlClient;
using System.Data;
using System.Text;
using Dapper;
using PiHoleUpdater.Common.Enums;
using PiHoleUpdater.Common.Models.Repo;
using PiHoleUpdater.Common.Models.Config;

namespace PiHoleUpdater.Common.Repo;

public interface IDomainRepo
{
  Task<IEnumerable<BlockListEntry>> GetEntriesAsync(AdList list);
  Task<IEnumerable<BlockListEntry>> GetEntriesByDomain(AdList list, string[] domains);
  Task<int> AssignDomainsToListAsync(AdList list, string[] domains);
  Task<int> AddEntriesAsync(AdList list, IEnumerable<BlockListEntry> entries);
  Task<int> UpdateSeenCountAsync(string[] domains);
  Task<IEnumerable<SimpleDomainEntity>> GetCompiledListAsync(AdList list);
  Task<IEnumerable<SimpleDomainEntity>> GetCompiledListAsync();
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
      d.`Domain`,
      {ListQueryHelper.GenerateSelectColumnName(list)}
    FROM `Domains` d
    WHERE
      {ListQueryHelper.GenerateWhereFilter(list, "d")}";

    return await _connection.QueryAsync<BlockListEntry>(query);
  }

  //public async Task<IEnumerable<BlockListEntry>> GetEntriesByDomain(AdList list, string[] domains)
  //{
  //  EnsureConnected();

  //  var query = @$"
  //  SELECT
  //    d.`Domain`,
  //    {ListQueryHelper.GenerateSelectColumnName(list)}
  //  FROM `Domains` d
  //  WHERE d.`Domain` IN @domains";

  //  return await _connection.QueryAsync<BlockListEntry>(query, new { domains });
  //}

  public async Task<IEnumerable<BlockListEntry>> GetEntriesByDomain(AdList list, string[] domains)
  {
    EnsureConnected();

    var query = @$"
    SELECT
      d.`Domain`,
      {ListQueryHelper.GenerateSelectColumnName(list)}
    FROM `Domains` d
    WHERE d.`Domain` IN ({GenerateDomainsList(domains)})";

    return await _connection.QueryAsync<BlockListEntry>(query);
  }

  //public async Task<int> AssignDomainsToListAsync(AdList list, string[] domains)
  //{
  //  EnsureConnected();

  //  var query = @$"
  //  UPDATE `Domains`
  //  SET
  //    `DateLastSeen` = current_timestamp(),
  //    `{ListQueryHelper.GetColumnName(list)}` = 1
  //  WHERE `Domain` IN @domains";

  //  return await _connection.ExecuteAsync(query, new { domains });
  //}

  public async Task<int> AssignDomainsToListAsync(AdList list, string[] domains)
  {
    EnsureConnected();

    var query = @$"
    UPDATE `Domains`
    SET
      `DateLastSeen` = current_timestamp(),
      `{ListQueryHelper.GetColumnName(list)}` = 1
    WHERE `Domain` IN ({GenerateDomainsList(domains)})";

    return await _connection.ExecuteAsync(query);
  }

  //public async Task<int> AddEntriesAsync(AdList list, IEnumerable<BlockListEntry> entries)
  //{
  //  EnsureConnected();

  //  var query = @$"
  //  INSERT INTO `Domains`
  //    (`Domain`, `{ListQueryHelper.GetColumnName(list)}`)
  //  VALUES
  //    (@Domain, 1)";

  //  return await _connection.ExecuteAsync(query, entries);
  //}

  public async Task<int> AddEntriesAsync(AdList list, IEnumerable<BlockListEntry> entries)
  {
    EnsureConnected();

    var domainRows = string.Join(",\n\t\t\t", entries.Select(e => $"('{e.Domain}', 1)"));

    var query = @$"
    INSERT INTO `Domains`
      (`Domain`, `{ListQueryHelper.GetColumnName(list)}`)
    VALUES
      {domainRows};";

    return await _connection.ExecuteAsync(query);
  }
  
  //public async Task<int> UpdateSeenCountAsync(string[] domains)
  //{
  //  EnsureConnected();

  //  const string query = @"
  //  UPDATE `Domains`
  //  SET
  //    `SeenCount` = `SeenCount` + 1,
  //    `DateLastSeen` = current_timestamp()
  //  WHERE
  //    `Domain` IN @domains";

  //  return await _connection.ExecuteAsync(query, new{ domains });
  //}

  public async Task<int> UpdateSeenCountAsync(string[] domains)
  {
    EnsureConnected();

    var query = @$"
    UPDATE `Domains`
    SET
      `SeenCount` = `SeenCount` + 1,
      `DateLastSeen` = current_timestamp()
    WHERE
      `Domain` IN ({GenerateDomainsList(domains)})";

    return await _connection.ExecuteAsync(query);
  }

  public async Task<IEnumerable<SimpleDomainEntity>> GetCompiledListAsync(AdList list)
  {
    EnsureConnected();

    var query = @$"
    SELECT
	    d.`Domain`
    FROM `Domains` d
    WHERE
	    d.`{ListQueryHelper.GetColumnName(list)}` = 1
    ORDER BY d.`Domain` ASC";

    return await _connection.QueryAsync<SimpleDomainEntity>(query);
  }

  public async Task<IEnumerable<SimpleDomainEntity>> GetCompiledListAsync()
  {
    EnsureConnected();

    const string query = @"
    SELECT
	    d.`Domain`
    FROM `Domains` d
    ORDER BY d.`Domain` ASC";

    return await _connection.QueryAsync<SimpleDomainEntity>(query);
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

  private string GenerateDomainsList(IEnumerable<string> domains) =>
    string.Join(",\n", domains.Select(d => $"'{d}'"));
}
