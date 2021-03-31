using System;
using System.Threading.Tasks;
using Identity.API.Dto;

namespace Identity.API.Services
{
  public interface IAuthService
  {
    Task<AuthResponse> Authenticate(AuthParam param);
  }
}
