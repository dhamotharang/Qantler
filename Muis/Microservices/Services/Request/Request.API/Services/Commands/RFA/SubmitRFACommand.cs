using Core.API;
using Core.API.Exceptions;
using Core.API.Repository;
using Core.EventBus;
using Core.Model;
using Request.API.Repository;
using Request.Events;
using Request.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Request.API.Services.Commands.RFA
{
  public class SubmitRFACommand : IUnitOfWorkCommand<Model.RFA>
  {
    readonly Model.RFA _rfa;
    
    readonly IEventBus _eventBus;

    public SubmitRFACommand(Model.RFA rfa, IEventBus eventBus)
    {
      _rfa = rfa;
      _eventBus = eventBus;
    }

    public async Task<Model.RFA> Invoke(IUnitOfWork unitOfWork)
    {
      var dbContext = DbContext.From(unitOfWork);
      unitOfWork.BeginTransaction();

      var hasPendingRFA = (await dbContext.RFA.Query(new RFAFilter
      {
        RequestID = _rfa.RequestID,
        Status = new List<RFAStatus> { RFAStatus.Open, RFAStatus.PendingReview }
      })).Any();

      if (hasPendingRFA)
      {
        throw new BadRequestException
          (await dbContext.Transalation.GetTranslation(Locale.EN, "NewRFAOpenExists"));
      }

      var request = await dbContext.Request.GetRequestByIDBasic(_rfa.RequestID);

      if (   request.Status != RequestStatus.Open
          && !(   request.Status == RequestStatus.ForInspection
               && request.StatusMinor == RequestStatusMinor.InspectionInProgress))
      {
        throw new BadRequestException
          (await dbContext.Transalation.GetTranslation(Locale.EN, "RFANotAllowed"));
      }

      var settingsType = request.Expedite ? SettingsType.RFAExpress : SettingsType.RFANormal;

      var settings = await dbContext.Settings.GetSettingsByType(settingsType);

      if (!int.TryParse(settings?.Value, out int workingDays))
      {
        workingDays = 7;
      }

      _rfa.DueOn = DateTimeOffset.UtcNow.AddDays(workingDays);

      var id = await dbContext.RFA.InsertRFA(_rfa);
      var result = await dbContext.RFA.GetRFAByID(id);

      if (_rfa.Status == RFAStatus.Open)
      {
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
