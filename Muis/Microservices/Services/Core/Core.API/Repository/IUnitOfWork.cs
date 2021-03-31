using System;
using System.Data;

namespace Core.API.Repository
{
  public interface IUnitOfWork
  {
    IDbConnection Connection { get; }

    IDbTransaction Transaction { get; }

    void BeginTransaction();

    void Commit();
  }
}
