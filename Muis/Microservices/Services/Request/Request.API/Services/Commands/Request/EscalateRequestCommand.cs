using Core.API;
using Core.API.Repository;
using Core.Model;
using Request.API.Repository;
using Request.Model;
using System;
using System.Threading.Tasks;

namespace Request.API.Services.Commands.Request
{
  public class EscalateRequestCommand : IUnitOfWorkCommand<Unit>
  {
    readonly long _requestID;
    readonly EscalateStatus _status;
    readonly string _remarks;

    readonly Guid _userID;
    readonly string _userName;

    public EscalateRequestCommand(long requestID, EscalateStatus status, string remarks,
      Guid userID, string userName)
    {
      _requestID = requestID;
      _status = status;
      _remarks = remarks;
      _userID = userID;
      _userName = userName;
    }

    public async Task<Unit> Invoke(IUnitOfWork unitOfWork)
    {
      await DbContext.From(unitOfWork).Request.EscalateAction(_requestID, _status, _remarks,
        _userID, _userName);

      return Unit.Default;
    }
  }
}
