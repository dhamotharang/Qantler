using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eHS.Portal.Client;

namespace eHS.Portal.Services.Bank
{
  public class BankService : IBankService
  {
    readonly ApiClient _apiClient;

    public BankService(ApiClient apiClient)
    {
      _apiClient = apiClient;
    }

    public async Task<IList<Model.Bank>> Filter(BankFilter filter)
    {
      return await _apiClient.BankSdk.List(filter);
    }
  }
}
