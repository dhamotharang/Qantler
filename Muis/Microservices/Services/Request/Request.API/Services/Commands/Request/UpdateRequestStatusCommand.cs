using System;
using System.Threading.Tasks;
using Core.API;
using Core.API.Repository;
using Core.Model;
using Request.API.Repository;
using Request.Model;

namespace Request.API.Services.Commands.Request
{
  public class UpdateRequestStatusCommand : IUnitOfWorkCommand<Unit>
  {
    readonly long _requestID;
    readonly RequestStatus _status;
    readonly RequestStatusMinor? _statusMinor;
    readonly Officer _user;

    public UpdateRequestStatusCommand(long requestID, RequestStatus status,
      RequestStatusMinor? statusMinor, Officer user)
    {
      _requestID = requestID;
      _status = status;
      _statusMinor = statusMinor;
      _user = user;
    }

    public async Task<Unit> Invoke(IUnitOfWork unitOfWork)
    {
      var dbContext = DbContext.From(unitOfWork);
      unitOfWork.BeginTransaction();

      string logText = null;

      switch(_status)
      {
        case RequestStatus.PendingPayment:

          logText = await dbContext.Transalation.GetTranslation(Locale.EN,
            "RequestSubmitForPayment");

          break;
      }

      if (   _user != null
          && !string.IsNullOrEmpty(logText))
      {
        var logID = await dbContext.Log.InsertLog(new Log
        {
          Action = logText,
          UserID = _user.ID,
          UserName = _user.Name
        });

        await dbContext.Request.MapLog(_requestID, logID);
      }

      await dbContext.Request.UpdateStatus(_requestID, _status, _statusMinor);

      unitOfWork.Commit();

      return Unit.Default;
    }
  }
}
