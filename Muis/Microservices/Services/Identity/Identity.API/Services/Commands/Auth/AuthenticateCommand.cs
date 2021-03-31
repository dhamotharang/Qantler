using System;
using System.Threading.Tasks;
using Core.API;
using Core.API.Exceptions;
using Core.API.Providers;
using Core.API.Repository;
using Core.Model;
using Core.Util;
using Identity.API.Dto;
using Identity.API.Repository;

namespace Identity.API.Services.Commands.Auth
{
  public class AuthenticateCommand : IUnitOfWorkCommand<AuthResponse>
  {
    readonly AuthParam _param;

    readonly ICacheProvider _cacheProvider;

    public AuthenticateCommand(AuthParam param, ICacheProvider cacheProvider)
    {
      _param = param;

      _cacheProvider = cacheProvider;
    }

    public async Task<AuthResponse> Invoke(IUnitOfWork uow)
    {
      var dbContext = DbContext.From(uow);

      var credential = await dbContext.Credential.GetCredential(_param.Provider,
        _param.Key,
        Encryptor.Encrypt(Secret.Key, _param.Secret));

      if (credential == null)
      {
        throw new BadRequestException("Invalid credentials");
      }

      var identity = await dbContext.Identity.GetIdentityByID(credential.IdentityID);

      if (identity.Status != Model.IdentityStatus.Active)
      {
        throw new BadRequestException(
          await dbContext.Translation.GetTranslation(Locale.EN, "AuthInactive"));
      }

      if (identity.Status == Model.IdentityStatus.Suspended)
      {
        throw new BadRequestException(
          await dbContext.Translation.GetTranslation(Locale.EN, "AuthSuspended"));
      }

      if (!string.IsNullOrEmpty(_param.NewSecret))
      {
        credential.Secret = Encryptor.Encrypt(Secret.Key, _param.NewSecret);
        credential.IsTemporary = false;

        await dbContext.Credential.UpdateCredential(credential);
      }
      else if (credential.IsTemporary)
      {
        return new AuthResponse { Action = AuthAction.ChangePassword };
      }

      // Store identity to cache
      await _cacheProvider.SetAsync(identity.ID.ToString(), "Ok");

      return new AuthResponse
      {
        Action = AuthAction.None,
        Identity = identity
      };
    }
  }
}
