using Case.API.Repository;
using Core.API;
using Core.API.Repository;
using Core.Model;
using System.Linq;
using System.Threading.Tasks;

namespace Case.API.Services.Commands.Case
{
  public class InsertActivityCommand : IUnitOfWorkCommand<long>
  {
    readonly Model.Activity _data;
    readonly Officer _user;
    readonly long _caseID;

    public InsertActivityCommand(Model.Activity data, Officer user, long caseID)
    {
      _data = data;
      _user = user;
      _caseID = caseID;
    }

    public async Task<long> Invoke(IUnitOfWork unitOfWork)
    {
      var dbContext = DbContext.From(unitOfWork);
      unitOfWork.BeginTransaction();

      _data.User = _user;

      await dbContext.User.InsertOrReplace(_user);

      _data.ID = await dbContext.Activity.InsertActivity(_data, _caseID);

      if (_data.Attachments?.Any() ?? false)
      {
        foreach (var attachment in _data.Attachments)
        {
          attachment.ID = await dbContext.Attachment.InsertAttachment(attachment);
        }

        await dbContext.Activity.MapActivityAttachments(
          _data.ID,
          _data.Attachments.Select(e => e.ID).ToArray());
      }

      unitOfWork.Commit();
      return _data.ID;
    }
  }
}
