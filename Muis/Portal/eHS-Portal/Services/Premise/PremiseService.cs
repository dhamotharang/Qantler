using eHS.Portal.Client;
using eHS.Portal.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eHS.Portal.Services
{
  public class PremiseService : IPremiseService
  {
    readonly ApiClient _apiClient;

    public PremiseService(ApiClient apiClient)
    {
      _apiClient = apiClient;
    }

    public async Task<IList<Premise>> GetPremise(Guid? customerID)
    {
      return await _apiClient.PremiseSdk.List(customerID);
    }

    public async Task<Premise> CreatePremise(Premise premise)
    {
      return await _apiClient.PremiseSdk.Create(premise);
    }
  }
}