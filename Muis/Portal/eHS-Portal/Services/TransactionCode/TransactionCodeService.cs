using eHS.Portal.Client;
using eHS.Portal.DTO;
using Core.Http.Exceptions;
using eHS.Portal.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eHS.Portal.Services.TransactionCode
{
  public class TransactionCodeService : ITransactionCodeService
  {
    readonly ApiClient _apiClient;

    public TransactionCodeService(ApiClient apiClient)
    {
      _apiClient = apiClient;
    }

    public async Task<IList<Model.TransactionCode>> Search(Model.TransactionCode options)
    {
      return await _apiClient.TransactionCodeSdk.Query(options);
    }

    public async Task<TransactionCodeDTO> GetByID(long id)
    {
      var result = new TransactionCodeDTO();

      result = await _apiClient.TransactionCodeSdk.GetByID(id);
      if (result == null)
      {
        throw new NotFoundException();
      }

      return result;
    }

    public async Task<bool> UpdatePrice(List<Price> priceDetails)
    {
      return await _apiClient.TransactionCodeSdk.UpdatePrice(priceDetails);
    }
  }
}
