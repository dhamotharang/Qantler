using System;
using System.Threading.Tasks;
using Core.API;
using Core.API.Provider;
using Core.Model;
using Request.API.Services.Commands;
using Request.Model;

namespace Request.API.Services
{
  public class EmailService : TransactionalService,
                              IEmailService
  {
    public EmailService(IDbConnectionProvider connectionProvider)
         : base(connectionProvider)
    {
    }

    public async Task<Email> GetByID(long id)
    {
      return await Execute(new GetEmailByIDCommand(id));
    }

    public async Task<Email> Save(Email email)
    {
      return await Execute(new SaveEmailCommand(email));
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
