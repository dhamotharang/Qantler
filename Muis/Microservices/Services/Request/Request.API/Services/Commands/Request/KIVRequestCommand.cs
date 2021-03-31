using System;
using System.Threading.Tasks;
using Core.API;
using Core.API.Repository;
using Request.API.Repository;

namespace Request.API.Services.Commands.Request
{
  public class KIVRequestCommand : IUnitOfWorkCommand<string>
  {
    readonly long _id;
    readonly string _notes;
    readonly DateTimeOffset _remindOn;
    readonly Guid _userID;
    readonly string _userName;

    public KIVRequestCommand(long id, string notes, DateTimeOffset remindOn, Guid userID,
      string userName)
    {
      _id = id;
      _notes = notes;
      _remindOn = remindOn;
      _userID = userID;
      _userName = userName;
    }

    public async Task<string> Invoke(IUnitOfWork unitOfWork)
    {
      var dbContext = DbContext.From(unitOfWork);
      unitOfWork.BeginTransaction();

      await dbContext.Request.KIV(_id, _notes, _remindOn, _userID, _userName);

      unitOfWork.Commit();

      return "OK";
    }
  }
}
