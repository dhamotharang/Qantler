using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using eHS.Portal.Client;

namespace eHS.Portal.Services.Bill
{
  public class BillService : IBillService
  {
    readonly ApiClient _apiClient;

    public BillService(ApiClient apiClient)
    {
      _apiClient = apiClient;
    }

    public async Task<IList<Model.Bill>> Filter(BillFilter filter)
    {
      return await _apiClient.BillSdk.List(filter);
    }

    public async Task<Model.Bill> GetByID(long id)
    {
      return await _apiClient.BillSdk.GetByID(id);
    }
  }
}
