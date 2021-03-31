using System;
using System.Threading.Tasks;
using Core.API;
using Core.API.Repository;
using Core.Model;
using Request.API.Repository;

namespace Request.API.Services.Commands.Request
{
  public class RevertKIVCommand : IUnitOfWorkCommand<Unit>
  {
    readonly long _id;
    readonly Guid _userID;
    readonly string _userName;

    public RevertKIVCommand(long id, Guid userID, string userName)
    {
      _id = id;
      _userID = userID;
      _userName = userName;
    }

    public async Task<Unit> Invoke(IUnitOfWork unitOfWork)
    {
      var dbContext = DbContext.From(unitOfWork);
      unitOfWork.BeginTransaction();

      await dbContext.Request.RevertKIV(_id, _userID, _userName);

      unitOfWork.Commit();

      return Unit.Default;
    }
  }
}
