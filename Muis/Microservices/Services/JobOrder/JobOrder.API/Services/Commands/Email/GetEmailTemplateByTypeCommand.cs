using System;
using System.Threading.Tasks;
using Core.API;
using Core.API.Repository;
using JobOrder.API.Repository;
using JobOrder.Model;

namespace JobOrder.API.Services.Commands
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
