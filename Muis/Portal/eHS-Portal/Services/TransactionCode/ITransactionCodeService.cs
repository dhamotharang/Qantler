using eHS.Portal.Client;
using eHS.Portal.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eHS.Portal.Services.TransactionCode
{
  public interface ITransactionCodeService
  {
    /// <summary>
    /// method to get all transaction code details
    /// </summary>
    /// <param name="options"></param>
    /// <returns></returns>
    Task<IList<Model.TransactionCode>> Search(Model.TransactionCode options);

    /// <summary>
    /// method to retirve transaction details by transaction id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<TransactionCodeDTO> GetByID(long id);

    /// <summary>
    /// method to update price details (Add and edit)
    /// </summary>
    /// <param name="priceDetails"></param>
    /// <returns></returns>
    Task<bool> UpdatePrice(List<Model.Price> priceDetails);

  }
}
