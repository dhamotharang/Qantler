using Case.API.Params;
using Case.API.Repository;
using Case.Model;
using Core.API;
using Core.API.Repository;
using Core.Model;
using System.Linq;
using System.Threading.Tasks;

namespace Case.API.Services.Commands.Case
{
  public class CaseAppealCommand : IUnitOfWorkCommand<long>
  {
    readonly long _caseID;
    readonly CaseAppealParam _data;
    readonly Officer _user;

    public CaseAppealCommand(long caseID, CaseAppealParam data, Officer user)
    {
      _caseID = caseID;
      _data = data;
      _user = user;
    }

    public async Task<long> Invoke(IUnitOfWork unitOfWork)
    {
      var dbContext = DbContext.From(unitOfWork);
      unitOfWork.BeginTransaction();

      var @case = await dbContext.Case.GetBasicByID(_caseID);
      @case.OldStatus = @case.Status;
      @case.Status = CaseStatus.PendingAppeal;
      @case.OtherStatus = CaseStatus.PendingAppeal;

      await dbContext.Case.UpdateCaseInfo(@case);

      var action = await dbContext.Translation.GetTranslation(Locale.EN, "SubmitAppeal");

      var _activityID = await dbContext.Activity.InsertActivity(new Activity
      {
        Action = action,
        User = _user,
        Notes = _data.Notes
      }, _caseID);

      if (_data.Attachments?.Any() ?? false)
      {
        foreach (var attachment in _data.Attachments)
        {
          attachment.ID = await dbContext.Attachment.InsertAttachment(attachment);
        }

        await dbContext.Activity.MapActivityAttachments(
          _activityID,
          _data.Attachments.Select(e => e.ID).ToArray());
      }

      unitOfWork.Commit();

      return _activityID;
    }
  }
}