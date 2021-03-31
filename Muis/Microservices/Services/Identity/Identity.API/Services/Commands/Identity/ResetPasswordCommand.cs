using System;
using System.Threading.Tasks;
using Core.API;
using Core.API.Exceptions;
using Core.API.Providers;
using Core.API.Repository;
using Core.API.Smtp;
using Core.Model;
using Core.Util;
using Identity.API.Repository;
using Identity.API.Util;
using Identity.Model;

namespace Identity.API.Services.Commands.Identity
{
  public class ResetPasswordCommand : IUnitOfWorkCommand<Unit>
  {
    readonly Guid _id;
    readonly Officer _user;

    readonly ICacheProvider _cacheProvider;
    readonly ISmtpProvider _smtpProvider;

    public ResetPasswordCommand(Guid id, Officer user, ICacheProvider cacheProvider,
      ISmtpProvider smtpProvider)
    {
      _id = id;
      _user = user;

      _cacheProvider = cacheProvider;
      _smtpProvider = smtpProvider;
    }

    public async Task<Unit> Invoke(IUnitOfWork uow)
    {
      var dbContext = DbContext.From(uow);

      var identity = await dbContext.Identity.GetIdentityByID(_id);
      if (identity == null)
      {
        throw new NotFoundException("Identity does not exists");
      }

      uow.BeginTransaction();

      var credential = await dbContext.Credential.GetCredentialByKey(Provider.Default,
        identity.Email);

      var tempSecret = RandomStringGenerator.Generate();

      credential.Secret = Encryptor.Encrypt(Secret.Key, tempSecret);
      credential.IsTemporary = true;

      await dbContext.Credential.UpdateCredential(credential);

      var logID = await dbContext.Log.InsertLog(new Log
      {
        Action = await dbContext.Translation.GetTranslation(Locale.EN, "IdentityPasswordReset"),
        UserID = _user.ID,
        UserName = _user.Name
      });

      await dbContext.Identity.MapIdentityToLog(identity.ID, logID);

      var template = await dbContext.EmailTemplate.GetTemplate(EmailTemplateType.ResetPassword);

      _smtpProvider.Send(new Mail
      {
        From = template.From,
        Recipients = new string[] { identity.Email },
        Subject = template.Title,
        Body = template.Body.Replace("{email}", identity.Email)
          .Replace("{username}", identity.Email.Replace("@muis.gov.sg", ""))
          .Replace("{secret}", tempSecret),
        IsBodyHtml = template.IsBodyHtml
      });

      // Remove from cache
      await _cacheProvider.RemoveAsync(identity.ID.ToString());

      uow.Commit();

      return Unit.Default;
    }
  }
}
