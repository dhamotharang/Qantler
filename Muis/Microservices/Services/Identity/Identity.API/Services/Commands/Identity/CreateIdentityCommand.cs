using System;
using System.Linq;
using System.Threading.Tasks;
using Core.API;
using Core.API.Exceptions;
using Core.API.Repository;
using Core.API.Smtp;
using Core.Model;
using Core.Util;
using Identity.API.Repository;
using Identity.API.Util;
using Identity.Model;

namespace Identity.API.Services.Commands.Identity
{
  public class CreateIdentityCommand : IUnitOfWorkCommand<Model.Identity>
  {
    readonly Officer _user;

    readonly Model.Identity _identity;

    readonly ISmtpProvider _smtpProvider;

    public CreateIdentityCommand(Model.Identity identity, Officer user, ISmtpProvider smtpProvider)
    {
      _user = user;
      _identity = identity;

      _smtpProvider = smtpProvider;
    }

    public async Task<Model.Identity> Invoke(IUnitOfWork uow)
    {
      var dbContext = DbContext.From(uow);

      var existing = await dbContext.Identity.Query(new IdentityFilter
      {
        Email = _identity.Email
      });

      if (existing?.FirstOrDefault(e => e.Email.ToLower().Equals(_identity.Email.ToLower())) != null)
      {
        throw new BadRequestException(string.Format(
          await dbContext.Translation.GetTranslation(Locale.EN, "IdentityEmailExsits"),
          _identity.Email));
      }

      uow.BeginTransaction();

      _identity.ID = Guid.NewGuid();

      // To fix permission length when stored to DB
      if ((_identity.Permissions?.Length ?? 0) < (int)Permission.SystemReadWrite)
      {
        PermissionUtil.SetPermission(_identity.Permissions,
          (int)Permission.SystemReadWrite,
          Access.None,
          out string fixedPermissions);

        _identity.Permissions = fixedPermissions;
      }

      await dbContext.Identity.Insert(_identity);

      await dbContext.Identity.MapIdentityToClusters(_identity.ID, _identity.Clusters);

      await dbContext.Identity.MapIdentityToRequestTypes(_identity.ID, _identity.RequestTypes);

      // Create credential

      var tempKey = RandomStringGenerator.Generate();

      await dbContext.Credential.InsertCredential(new Credential
      {
        Key = _identity.Email.ToLower(),
        Secret = Encryptor.Encrypt(Secret.Key, tempKey),
        Provider = Provider.Default,
        IdentityID = _identity.ID,
        IsTemporary = true
      });

      var template = await dbContext.EmailTemplate.GetTemplate(EmailTemplateType.NewAccount);

      _smtpProvider.Send(new Mail
      {
        From = template.From,
        Recipients = new string[] { _identity.Email },
        Subject = template.Title,
        Body = template.Body.Replace("{email}", _identity.Email)
          .Replace("{username}", _identity.Email.Replace("@muis.gov.sg", ""))
          .Replace("{secret}", tempKey),
        IsBodyHtml = template.IsBodyHtml
      });

      var logID = await dbContext.Log.InsertLog(new Log
      {
        Action = await dbContext.Translation.GetTranslation(Locale.EN, "IdentityCreated"),
        UserID = _user.ID,
        UserName = _user.Name
      });

      await dbContext.Identity.MapIdentityToLog(_identity.ID, logID);

      var result = await dbContext.Identity.GetIdentityByID(_identity.ID);

      uow.Commit();

      return result;
    }
  }
}
