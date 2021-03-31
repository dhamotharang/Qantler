using System;
using System.Data;

namespace Core.Base.Repository
{
  public interface IDbConnectionProvider
  {
    IDbConnection Connection { get; }
  }
}
