using System;
using System.Threading.Tasks;
using Core.API;
using Core.API.Repository;
using Case.API.Repository;
using Case.Model;

namespace Case.API.Services.Commands
{
  public class GetEmailTemplateByTypeCommand : IUnitOfWorkCommand<EmailTemplate>
  {
    readonly EmailTemplateType _type;

    public GetEmailTemplateByTypeCommand(EmailTemplateType type)
    {
      _type = type;
    }

    public async Task<EmailTemplate> Invoke(IUnitOfWork unitOfWork)
    {
      return await DbContext.From(unitOfWork).Email.GetTemplate(_type);
    }
  }
}
