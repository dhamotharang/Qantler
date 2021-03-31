using System;
using System.Threading.Tasks;
using eHS.Portal.Client;
using eHS.Portal.DTO;

namespace eHS.Portal.Services.Auth
{
  public class AuthService : IAuthService
  {
    readonly ApiClient _apiClient;

    public AuthService(ApiClient apiClient)
    {
      _apiClient = apiClient;
    }

    public async Task<AuthResponse> Authenticate(AuthParam param)
    {
      return await _apiClient.AuthSdk.Authenticate(param);
    }
  }
}
