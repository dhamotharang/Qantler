using Finance.API.Services;
using Finance.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Finance.API.Repository
{
  public interface IBankRepository
  {
    /// <summary>
    /// insert bank
    /// </summary>
    public Task<long> Insert(Bank bank);

    /// <summary>
    /// update bank
    /// </summary>
    public Task Update(Bank bank);

    /// <summary>
    /// select bank
    /// </summary>
    public Task<IList<Bank>> Query(BankFilter options);

    /// <summary>
    /// delete bank
    /// </summary>
    public Task Delete(long id);
  }
}
