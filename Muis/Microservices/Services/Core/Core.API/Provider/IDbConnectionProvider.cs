using System;
using System.Data;

namespace Core.API.Provider
{
  public interface IDbConnectionProvider
  {
    IDbConnection Connection { get; }
  }
}
