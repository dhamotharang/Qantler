using Core.API;
using Core.API.Provider;
using Core.EventBus;
using Request.API.Services.Commands.RFA;
using Request.Model;
using System;
using System.Threading.Tasks;
using Request.API.Repository;
using System.Collections.Generic;

namespace Request.API.Services
{
  public class RFAService : TransactionalService,
                            IRFAService
  {
    readonly IEventBus _eventBus;

    public RFAService(IDbConnectionProvider connectionProvider, IEventBus eventBus)
         : base(connectionProvider)
    {
      _eventBus = eventBus;
    }

    public async Task<RFA> GetRFAByID(long id)
    {
      return await Execute(new GetRFAByIDCommand(id));
    }

    public async Task<RFA> SubmitRFA(RFA rfa)
    {
      if (rfa.ID != 0L)
      {
        return await Execute(new UpdateRFACommand(rfa, _eventBus));
      }
      return await Execute(new SubmitRFACommand(rfa, _eventBus));
    }

    public async Task<RFA> SubmitRFAResponse(RFA rfa)
    {
      return await Execute(new SubmitRFAResponseCommand(rfa, _eventBus));
    }

    public async Task UpdateRFAStatus(long id, RFAStatus rfaStatus, Guid userid,
      string username)
    {
      await Execute(new UpdateRFAStatusCommand(id, userid, username, rfaStatus, _eventBus));
    }

    public async Task ExtendDueDate(long id, string notes, DateTimeOffset toDate, Guid userID,
      string userName)
    {
      await Execute(new ExtendRFADueDateCommand(id, notes, toDate, userID, userName, _eventBus));
    }

    public async Task<bool> Delete(long id)
    {
      return await Execute(new DeleteRFACommand(id));
    }

    public Task<IList<RFA>> ListOfRFA(RFAFilter filter)
    {
      return Execute(new ListOfRFACommand(filter));
    }
  }
}
