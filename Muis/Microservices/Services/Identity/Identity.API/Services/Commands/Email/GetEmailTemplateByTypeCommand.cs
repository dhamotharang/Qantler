using Core.API;
using Core.API.Repository;
using Identity.API.Repository;
using Identity.Model;
using System.Threading.Tasks;

namespace Identity.API.Services.Commands.Email
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
      return await DbContext.From(unitOfWork).EmailTemplate.GetTemplate(_type);
    }
  }
}
