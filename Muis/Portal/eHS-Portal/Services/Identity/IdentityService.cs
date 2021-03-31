using eHS.Portal.Client;
using eHS.Portal.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eHS.Portal.Services
{
  public class IdentityService : IIdentityService
  {
    readonly ApiClient _apiClient;

    public IdentityService(ApiClient apiClient)
    {
      _apiClient = apiClient;
    }

    public async Task<Identity> CreateUser(Identity identity)
    {
      return await _apiClient.IdentitySdk.CreateIdentity(identity);
    }

    public async Task<Identity> UpdateUser(Identity identity)
    {
      return await _apiClient.IdentitySdk.UpdateIdentity(identity);
    }

    public async Task<Identity> GetIdentityByID(Guid id)
    {
      return await _apiClient.IdentitySdk.GetIdentityByID(id);
    }

    public async Task<IList<Identity>> List(IdentityFilter filter)
    {
      return await _apiClient.IdentitySdk.List(filter);
    }

    public async Task ResetPassword(Guid id)
    {
      await _apiClient.IdentitySdk.ResetPassword(id);
    }

    public async Task ForgotPassword(string email)
    {
      await _apiClient.IdentitySdk.ForgotPassword(email);
    }
  }
}