using Core.API;
using Core.API.Repository;
using Core.Model;
using Request.API.Repository;
using Request.Model;
using System;
using System.Threading.Tasks;

namespace Request.API.Services.Commands
{
  public class UpdateEmailTemplateCommand : IUnitOfWorkCommand<Unit>
  {
    readonly EmailTemplate _emailTemplate;

    readonly Guid _id;

    readonly string _userName;

    public UpdateEmailTemplateCommand(EmailTemplate emailTemplateDetails, Guid id,
      string userName)
    {
      _emailTemplate = emailTemplateDetails;
      _id = id;
      _userName = userName;
    }

    public async Task<Unit> Invoke(IUnitOfWork unitOfWork)
    {
      unitOfWork.BeginTransaction();

      await DbContext.From(unitOfWork).Email.UpdateTemplate(_emailTemplate, _id, _userName);

      unitOfWork.Commit();

      return Unit.Default;
    }
  }
}
