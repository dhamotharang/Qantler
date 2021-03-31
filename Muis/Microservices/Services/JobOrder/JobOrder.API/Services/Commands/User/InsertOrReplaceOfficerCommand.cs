using System;
using System.Threading.Tasks;
using Core.API;
using Core.API.Repository;
using Core.Model;
using JobOrder.API.Repository;

namespace JobOrder.API.Services.Commands.User
{
  public class InsertOrReplaceOfficerCommand : IUnitOfWorkCommand<Unit>
  {
    readonly Officer _user;

    public InsertOrReplaceOfficerCommand(Officer user)
    {
      _user = user;
    }

    public async Task<Unit> Invoke(IUnitOfWork uow)
    {
      var dbContext = DbContext.From(uow);
      uow.BeginTransaction();

      await dbContext.User.InsertOrReplace(_user);

      uow.Commit();

      return Unit.Default;
    }
  }
}
