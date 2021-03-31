using System;
using System.Data;

namespace Core.Base.Repository
{
  public interface IUnitOfWork
  {
    IDbConnection Connection { get; }

    IDbTransaction Transaction { get; }

    void BeginTransaction();

    void Commit();
  }
}
