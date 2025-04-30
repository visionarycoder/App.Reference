using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Praetor.Rules.Abstractions;
using Praetor.Rules.Model;

namespace Praetor.Rules.Sql;

public sealed class SqlRuleStore(string connStr) : IRuleStore
{

  const string SelectAll = "SELECT Name, JsonBody, UpdatedUtc FROM RuleDefinitions;";
  const string SelectOne = "SELECT Name, JsonBody, UpdatedUtc FROM RuleDefinitions WHERE Name = @name;";

  static RuleDefinition Map(IDataRecord r) => new(r.GetString(0), r.GetString(1), r.GetDateTimeOffset(2));

  async Task<IReadOnlyList<RuleDefinition>> IRuleStore.GetAllAsync(CancellationToken ct)
  {
    var list = new List<RuleDefinition>();
    await using DbConnection conn = new SqlConnection(connStr);
    await conn.OpenAsync(ct);
    await using var cmd = new SqlCommand(SelectAll, (SqlConnection) conn);
    await using var rdr = await cmd.ExecuteReaderAsync(ct);

    while(await rdr.ReadAsync(ct))
      list.Add(Map(rdr));

    return list;
  }

  async Task<RuleDefinition?> IRuleStore.GetAsync(string name, CancellationToken ct)
  {
    await using DbConnection conn = new SqlConnection(connStr);
    await conn.OpenAsync(ct);
    await using var cmd = new SqlCommand(SelectOne, (SqlConnection) conn);
    cmd.Parameters.AddWithValue("@name", name);
    await using var rdr = await cmd.ExecuteReaderAsync(ct);
    return await rdr.ReadAsync(ct) ? Map(rdr) : null;
  }
}
