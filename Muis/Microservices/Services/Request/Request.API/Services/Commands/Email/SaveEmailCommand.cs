using System;
using System.Threading.Tasks;
using Core.API;
using Core.API.Repository;
using Core.Model;
using Request.API.Repository;

namespace Request.API.Services.Commands
{
  public class SaveEmailCommand : IUnitOfWorkCommand<Email>
  {
    readonly Email _email;

    public SaveEmailCommand(Email email)
    {
      _email = email;
    }

    public async Task<Email> Invoke(IUnitOfWork unitOfWork)
    {
      var dbContext = DbContext.From(unitOfWork);
      unitOfWork.BeginTransaction();

      var id = await dbContext.Email.Insert(_email);

      var result =  await dbContext.Email.GetByID(id);

      unitOfWork.Commit();

      return result;
    }
  }
}
