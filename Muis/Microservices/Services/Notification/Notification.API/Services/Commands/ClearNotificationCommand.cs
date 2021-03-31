using System;
using System.Threading.Tasks;
using Core.API;
using Core.API.Repository;
using Core.Model;
using Notification.API.Repository;

namespace Notification.API.Services.Commands
{
  public class ClearNotificationCommand : IUnitOfWorkCommand<Unit>
  {
    readonly Guid _userID;

    public ClearNotificationCommand(Guid userID)
    {
      _userID = userID;
    }

    public async Task<Unit> Invoke(IUnitOfWork unitOfWork)
    {
      var dbContext = new DbContext(unitOfWork);

      await dbContext.Notification.Clear(_userID);

      return Unit.Default;
    }
  }
}
