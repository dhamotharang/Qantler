using System;
using System.Threading.Tasks;
using Finance.Model;

namespace Finance.API.Repository
{
  public interface IAccountRepository
  {
    /// <summary>
    /// Insert or replace account entity.
    /// </summary>
    Task InsertOrReplace(Account entity);
  }
}
