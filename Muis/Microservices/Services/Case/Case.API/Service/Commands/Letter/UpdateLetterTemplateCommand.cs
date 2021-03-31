using Core.API;
using Core.API.Repository;
using Core.Model;
using Case.API.Repository;
using Case.Model;
using System;
using System.Threading.Tasks;

namespace Case.API.Services.Commands
{
  public class UpdateLetterTemplateCommand : IUnitOfWorkCommand<Unit>
  {
    readonly LetterTemplate _template;

    readonly Guid _id;

    readonly string _userName;

    public UpdateLetterTemplateCommand(LetterTemplate template, Guid id,
      string userName)
    {
      _template = template;
      _id = id;
      _userName = userName;
    }

    public async Task<Unit> Invoke(IUnitOfWork unitOfWork)
    {
      unitOfWork.BeginTransaction();

      await DbContext.From(unitOfWork).Letter.UpdateTemplate(_template, _id, _userName);

      unitOfWork.Commit();

      return Unit.Default;
    }
  }
}
