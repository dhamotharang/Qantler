using System;
using System.Data;

namespace Core.Base.Repository
{
  public class UnitOfWork : IUnitOfWork,
                            IDisposable
  {
    IDbConnection _connection;
    public IDbConnection Connection => _connection ??= _connectionProvider.Connection;

    public IDbTransaction Transaction { get; private set; }

    readonly IDbConnectionProvider _connectionProvider;

    public UnitOfWork(IDbConnectionProvider connectionProvider)
    {
      _connectionProvider = connectionProvider;
    }

    public void BeginTransaction()
    {
      if (Transaction != null)
      {
        throw new Exception("New transaction is not allowed. An active transaction already exists.");
      }

      Transaction = Connection.BeginTransaction();
    }

    public void Commit()
    {
      Transaction?.Commit();
    }

    public void Rollback()
    {
      Transaction?.Rollback();
    }

    public void Dispose()
    {
      Transaction?.Dispose();
      Transaction = null;

      _connection?.Dispose();
      _connection = null;
    }
  }
}
