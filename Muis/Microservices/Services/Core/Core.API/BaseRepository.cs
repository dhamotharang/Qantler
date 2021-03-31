using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Options;

namespace Core.API
{
  public delegate Task<T> Transaction<T>(IDbConnection conn, IDbTransaction trx);

  public delegate Task<T> Action<T>(IDbConnection conn);

  public class BaseRepository
  {
    readonly string _connectionString;

    protected IDbConnection Connection { get; set; }

    public BaseRepository(IOptions<ConnectionStrings> connectionStrings)
    {
      _connectionString = connectionStrings.Value.Default;

      Connection = new SqlConnection(connectionStrings.Value.Default);
      SqlMapper.AddTypeHandler(new UtcTypeHandler());
    }

    public void Dispose()
    {
      Connection.Close();
      Connection = null;
    }

    public async Task<T> Execute<T>(Action<T> action)
    {
      T result = default(T);

      using (var conn = new SqlConnection(_connectionString))
      {
        await conn.OpenAsync();

        result = await action(conn);
      }

      return result;
    }

    public async Task<T> ExecuteInTransaction<T>(Transaction<T> transaction)
    {
      T result = default(T);

      using (var conn = new SqlConnection(_connectionString))
      {
        await conn.OpenAsync();

        using (var trx = conn.BeginTransaction())
        {
          try
          {
            result = await transaction(conn, trx);
            trx.Commit();
          }
          catch (Exception e)
          {
            trx.Rollback();
            throw e;
          }
        }
      }

      return result;
    }
  }

  class UtcTypeHandler : SqlMapper.TypeHandler<DateTimeOffset>
  {
    public override void SetValue(IDbDataParameter parameter, DateTimeOffset value)
    {
      parameter.Value = value;
    }

    public override DateTimeOffset Parse(object value)
    {
      return DateTime.SpecifyKind((DateTime)value, DateTimeKind.Utc);
    }
  }
}
