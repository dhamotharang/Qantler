using Finance.API.Models;
using Finance.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Finance.API.Services
{
  public interface ITransactionCodeService
  {
    /// <summary>
    /// Get all Finance Transaction code details
    /// </summary>
    /// <param name="options"></param>
    /// <returns></returns>
    Task<IEnumerable<TransactionCode>> QueryTransactionCode(
      TransactionCodeOptions options);

    /// <summary>
    /// Get Transaction Code details by ID
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<TransactionCode> GetTransactionCodeByID(long id);

    /// <summary>
    /// update existing price details
    /// </summary>
    /// <param name="priceList"></param>
    /// <param name="userID"></param>
    /// <param name="userName"></param>
    /// <returns></returns>
    Task<bool> UpdatePrice(IList<Price> priceList, Guid userID, string userName);
  }
}
