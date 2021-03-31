using System;
using System.Threading.Tasks;
using eHS.Portal.DTO;

namespace eHS.Portal.Services.Auth
{
  public interface IAuthService
  {
    /// <summary>
    /// Authenticate.
    /// </summary>
    Task<AuthResponse> Authenticate(AuthParam param);
  }
}
