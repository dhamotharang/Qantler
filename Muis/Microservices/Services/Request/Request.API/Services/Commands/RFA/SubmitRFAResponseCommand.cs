using Core.API;
using Core.API.Repository;
using Request.API.Repository;
using System.Threading.Tasks;
using Core.Model;
using Core.EventBus;
using Request.Events;
using System.Collections.Generic;

namespace Request.API.Services.Commands.RFA
{
  public class SubmitRFAResponseCommand : IUnitOfWorkCommand<Model.RFA>
  {
    readonly Model.RFA _rfa;
    readonly IEventBus _eventBus;

    public SubmitRFAResponseCommand(Model.RFA rfa, IEventBus eventBus)
    {
      _rfa = rfa;
      _eventBus = eventBus;
    }

    public async Task<Model.RFA> Invoke(IUnitOfWork uow)
    {
      var dbContext = DbContext.From(uow);
      uow.BeginTransaction();

      var id = await dbContext.RFA.InsertRFAResponse(_rfa);

      var logText = string.Format(
       await dbContext.Transalation.GetTranslation(0, "RFARespondBody"));

      var logID = await dbContext.Log.InsertLog(new Log
      {
        Action = logText
      });

      var result = await dbContext.RFA.GetRFAByID(id);

      var request = await dbContext.Request.GetRequestByID(result.RequestID);

      _eventBus.Publish(new SendNotificationEvent
      {
        Title = await dbContext.Transalation.GetTranslation(0, "RFARespondTitle"),
        Body = await dbContext.Transalation.GetTranslation(0, "RFARespondBody"),
        Module = "Request",
        To = new List<string> { request.AssignedTo.ToString() }
      });

      uow.Commit();

      return result;
    }
  }
}
