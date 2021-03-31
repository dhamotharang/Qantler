using System;
using System.Threading.Tasks;
using Core.API;
using Core.API.Repository;
using Core.EventBus;
using Request.API.Repository;
using Request.Events;

namespace Request.API.Services.Commands.RFA
{
  public class UpdateRFACommand : IUnitOfWorkCommand<Model.RFA>
  {
    readonly Model.RFA _rfa;

    readonly IEventBus _eventBus;

    public UpdateRFACommand(Model.RFA rfa, IEventBus eventBus)
    {
      _rfa = rfa;
      _eventBus = eventBus;
    }

    public async Task<Model.RFA> Invoke(IUnitOfWork unitOfWork)
    {
      var dbContext = DbContext.From(unitOfWork);
      unitOfWork.BeginTransaction();

      await dbContext.RFA.UpdateRFA(_rfa);
      var result = await dbContext.RFA.GetRFAByID(_rfa.ID);

      if (_rfa.Status == Model.RFAStatus.Open)
      {
        var request = await dbContext.Request.GetRequestByIDBasic(_rfa.RequestID);

        _eventBus.Publish(new OnNewRFAEvent
        {
          ID = result.ID,
          RequestID = result.RequestID,
          RefID = request.RefID
        });
      }

      unitOfWork.Commit();

      return result;
    }
  }
}
