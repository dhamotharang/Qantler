using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using eHS.Portal.Client;
using eHS.Portal.Model;

namespace eHS.Portal.Services.Certificate360
{
  public class Certificate360Service : ICertificate360Service
  {
    readonly ApiClient _apiClient;

    public Certificate360Service(ApiClient apiClient)
    {
      _apiClient = apiClient;
    }

    public async Task<Model.Certificate360> GetByID(long id)
    {
      return await _apiClient.Certificate360.GetByID(id);
    }

    public async Task<IList<Certificate360History>> GetHistoryByID(long id)
    {
      return await _apiClient.Certificate360.GetHistoryByID(id);
    }

    public async Task<IList<Menu>> GetMenuByID(long id)
    {
      return await _apiClient.Certificate360.GetMenuByID(id);
    }

    public async Task<IList<Ingredient>> GetIngredientByID(long id)
    {
      return await _apiClient.Certificate360.GetIngredientByID(id);
    }

    public async Task<IList<Model.Certificate360>> GetWithIngredient
      (Certificate360IngredientOptions options)
    {
      return await _apiClient.Certificate360.GetWithIngredient(options);
    }
  }
}
