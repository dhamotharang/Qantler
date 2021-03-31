using System;
using System.Threading.Tasks;
using Core.API;
using Core.API.Provider;
using Core.API.Providers;
using Identity.API.Dto;
using Identity.API.Services.Commands.Auth;

namespace Identity.API.Services
{
  public class AuthService : TransactionalService,
                             IAuthService
  {
    readonly ICacheProvider _cacheProvider;

    public AuthService(IDbConnectionProvider connectionProvider, ICacheProvider cacheProvider)
         : base(connectionProvider)
    {
      _cacheProvider = cacheProvider;
    }

    public async Task<AuthResponse> Authenticate(AuthParam param)
    {
      return await Execute(new AuthenticateCommand(param, _cacheProvider));
    }
  }
}
