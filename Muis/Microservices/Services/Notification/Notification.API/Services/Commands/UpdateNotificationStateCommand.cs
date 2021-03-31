using System;
using System.Threading.Tasks;
using Core.API;
using Core.API.Repository;
using Core.Model;
using Notification.API.Repository;
using Notification.Model;

namespace Notification.API.Services.Commands
{
  public class UpdateNotificationStateCommand : IUnitOfWorkCommand<Unit>
  {
    readonly long _notificationID;
    readonly Guid _userID;
    readonly State _state;

    public UpdateNotificationStateCommand(long notificationID, Guid userID, State state)
    {
      _notificationID = notificationID;
      _userID = userID;
      _state = state;
    }

    public async Task<Unit> Invoke(IUnitOfWork unitOfWork)
    {
      var dbContex = new DbContext(unitOfWork);

      await dbContex.Notification.UpdateState(_notificationID, _userID, _state);

      return Unit.Default;
    }
  }
}
