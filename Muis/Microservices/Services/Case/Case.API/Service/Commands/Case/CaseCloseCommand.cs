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
  public class CaseCloseCommand : IUnitOfWorkCommand<long>
  {
    readonly long _caseID;
    readonly CaseClose _data;
    readonly Officer _user;

    public CaseCloseCommand(long caseID, CaseClose data, Officer user)
    {
      _caseID = caseID;
      _data = data;
      _user = user;
    }

    public async Task<long> Invoke(IUnitOfWork unitOfWork)
    {
      var dbContext = DbContext.From(unitOfWork);
      unitOfWork.BeginTransaction();

      await dbContext.Case.UpdateStatus(_caseID, CaseStatus.Closed);

      var action = await dbContext.Translation.GetTranslation(Locale.EN, "CaseClose");

      var activityID = await dbContext.Activity.InsertActivity(new Activity
      {
        Type = ActivityType.Default,
        Action = action,
        Notes = _data.Notes,
        User = _user
      }, _caseID);

      if (_data.Attachments?.Any() ?? false)
      {
        foreach (var attachment in _data.Attachments)
        {
          attachment.ID = await dbContext.Attachment.InsertAttachment(attachment);
        }

        await dbContext.Activity.MapActivityAttachments(
          activityID,
          _data.Attachments.Select(e => e.ID).ToArray());
      }

      unitOfWork.Commit();

      return activityID;
    }
  }
}