using Core.API;
using Core.API.Provider;
using Identity.API.Services.Commands.Email;
using Identity.Model;
using System;
using System.Threading.Tasks;

namespace Identity.API.Services
{
  public class EmailService : TransactionalService,
                               IEmailService
  {
    public EmailService(IDbConnectionProvider connectionProvider)
         : base(connectionProvider)
    {
    }

    public async Task<EmailTemplate> GetTemplate(EmailTemplateType type)
    {
      return await Execute(new GetEmailTemplateByTypeCommand(type));
    }

    public async Task UpdateTemplate(
      EmailTemplate emailTemplateDetails, Guid id, string userName)
    {
      await Execute(new UpdateEmailTemplateCommand(emailTemplateDetails, id, userName));
    }
  }
}
