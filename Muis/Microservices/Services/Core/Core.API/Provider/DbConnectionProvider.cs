using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace Core.API.Provider
{
  public class DbConnectionProvider : IDbConnectionProvider
  {
    readonly ConnectionStrings _connectionStrings;

    public DbConnectionProvider(IOptions<ConnectionStrings> connectionStrings)
    {
      _connectionStrings = connectionStrings.Value;
    }

    public IDbConnection Connection
    {
      get
      {
        var conn = new SqlConnection(_connectionStrings.Default);
        conn.Open();
        return conn;
      }
    }
  }
}
