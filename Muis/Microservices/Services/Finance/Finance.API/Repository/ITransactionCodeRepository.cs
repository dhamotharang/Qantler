using Finance.API.Models;
using Finance.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Finance.API.Repository
{
  public interface ITransactionCodeRepository
  {
    /// <summary>
    /// Get Finance transaction code by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Task<TransactionCode> GetTransactionCodeByID(long id);

    /// <summary>
    /// Get all the transaction code details
    /// </summary>
    /// <param name="options"></param>
    /// <returns></returns>
    public Task<IEnumerable<TransactionCode>> Select(
      TransactionCodeOptions options);

    /// <summary>
    /// update existing price
    /// </summary>
    /// <param name="priceList"></param>
    /// <param name="userID"></param>
    /// <param name="userName"></param>
    /// <returns></returns>
    Task<bool> UpdatePrice(IList<Price> priceList, Guid userID, string userName);

    /// <summary>
    /// Retrieve latest price for specified code and reference date.
    /// </summary>
    /// <param name="code">transaction code</param>
    /// <param name="refDate">reference date</param>
    /// <returns>the price</returns>
    Task<Price> GetLatestPrice(string code, DateTimeOffset refDate);

    /// <summary>
    /// Retrieve transaction code base on specified code.
    /// </summary>
    /// <param name="code">transaction code</param>
    /// <param name="refDate">reference date</param>
    /// <returns>the price</returns>
    Task<TransactionCode> GetTransactionCodeByCode(string code, DateTimeOffset? refDate = null);
  }
}