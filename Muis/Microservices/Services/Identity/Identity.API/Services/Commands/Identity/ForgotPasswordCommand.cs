using System;
using System.Linq;
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
  public class ForgotPasswordCommand : IUnitOfWorkCommand<Unit>
  {
    readonly string _email;

    readonly ICacheProvider _cacheProvider;
    readonly ISmtpProvider _smtpProvider;

    public ForgotPasswordCommand(string email, ICacheProvider cacheProvider,
      ISmtpProvider smtpProvider)
    {
      _email = email;

      _cacheProvider = cacheProvider;
      _smtpProvider = smtpProvider;
    }

    public async Task<Unit> Invoke(IUnitOfWork uow)
    {
      var dbContext = DbContext.From(uow);

      var identity = (await dbContext.Identity.Query(new IdentityFilter
      {
        Email = _email
      })).FirstOrDefault();

      if (identity == null)
      {
        throw new BadRequestException("Identity does not exists.");
      }

      uow.BeginTransaction();

      var credential = await dbContext.Credential.GetCredentialByKey(Provider.Default,
        identity.Email);

      var tempSecret = RandomStringGenerator.Generate();

      credential.Secret = Encryptor.Encrypt(Secret.Key, tempSecret);
      credential.IsTemporary = true;

      await dbContext.Credential.UpdateCredential(credential);

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
