using Core.API;
using Core.API.Repository;
using Core.EventBus;
using Request.API.Repository;
using Request.Events;
using System;
using System.Threading.Tasks;

namespace Request.API.Services.Commands.RFA
{
  public class ExtendRFADueDateCommand : IUnitOfWorkCommand<Model.RFA>
  {
    readonly long _id;
    readonly string _notes;
    readonly Guid _userID;
    readonly string _userName;
    readonly DateTimeOffset _toDate;

    readonly IEventBus _eventBus;

    public ExtendRFADueDateCommand(long id, string notes, DateTimeOffset toDate, Guid userID,
      string userName, IEventBus eventBus)
    {
      _id = id;
      _notes = notes;
      _toDate = toDate;
      _userID = userID;
      _userName = userName;
      _eventBus = eventBus;
    }

    public async Task<Model.RFA> Invoke(IUnitOfWork unitOfWork)
    {
      var dbContext = DbContext.From(unitOfWork);
      unitOfWork.BeginTransaction();

      await dbContext.RFA.ExtendDueDate(_id, _notes, _toDate, _userID, _userName);
      var result = await dbContext.RFA.GetRFAByID(_id);
      var request = await dbContext.Request.GetRequestByIDBasic(result.RequestID);

      _eventBus.Publish(new OnRFAExtendedEvent
      {
        RFAID = result.ID,
        RequestID = result.RequestID,
        RefID = request.RefID,
      });

      unitOfWork.Commit();

      return result;
    }

  }
}
