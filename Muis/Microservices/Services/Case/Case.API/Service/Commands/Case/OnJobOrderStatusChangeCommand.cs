using Case.API.Repository;
using Case.Events;
using Case.Model;
using Core.API;
using Core.API.Repository;
using Core.Model;
using System.Threading.Tasks;

namespace Case.API.Services.Commands.Case
{
  public class OnJobOrderStatusChangeCommand : IUnitOfWorkCommand<Unit>
  {
    readonly JobOrderStatus _inspectionStatus;
    readonly long _caseID;

    public OnJobOrderStatusChangeCommand(JobOrderStatus inspectionStatus, long caseID)
    {
      _inspectionStatus = inspectionStatus;
      _caseID = caseID;
    }

    public async Task<Unit> Invoke(IUnitOfWork unitOfWork)
    {
      if (_inspectionStatus != JobOrderStatus.Done
        && _inspectionStatus != JobOrderStatus.Cancelled)
      {
        return Unit.Default;
      }

      var dbContext = DbContext.From(unitOfWork);
      unitOfWork.BeginTransaction();
      var action = "";

      var @case = await dbContext.Case.GetBasicByID(_caseID);

      if (@case == null
          || (@case.Status != CaseStatus.PendingInspection
          && @case.Status != CaseStatus.PendingReinstateInspection
          && @case.MinorStatus != null))
      {
        return Unit.Default;
      }

      if (_inspectionStatus == JobOrderStatus.Cancelled)
      {
        action = await dbContext.Translation.GetTranslation(Locale.EN, "ScheduledInspectionCancelled");

        await dbContext.Case.UpdateStatus(_caseID, @case.Status, CaseMinorStatus.InspectionCancelled);
      }
      else if (_inspectionStatus == JobOrderStatus.Done)
      {
        action = await dbContext.Translation.GetTranslation(Locale.EN, "ScheduledInspectionCompleted");

        await dbContext.Case.UpdateStatus(_caseID, @case.Status, CaseMinorStatus.InspectionDone);
      }

      await dbContext.Activity.InsertActivity(new Activity
      {
        Action = action
      }, _caseID);

      unitOfWork.Commit();

      return Unit.Default;
    }
  }
}