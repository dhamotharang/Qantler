using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.API;
using Core.API.Repository;
using Core.EventBus;
using Core.Model;
using Request.API.Repository;
using Request.Events;
using Request.Model;

namespace Request.API.Services.Commands.Cert
{
  public class AddCommentCommand : IUnitOfWorkCommand<Comment>
  {
    readonly long _batchID;
    readonly string _text;
    readonly Officer _user;

    readonly IEventBus _eventBus;

    public AddCommentCommand(long batchID, string text, Officer user, IEventBus eventBus)
    {
      _batchID = batchID;
      _text = text;
      _user = user;

      _eventBus = eventBus;
    }

    public async Task<Comment> Invoke(IUnitOfWork unitOfWork)
    {
      var dbContext = DbContext.From(unitOfWork);
      unitOfWork.BeginTransaction();

      var id = await dbContext.Certificate.InsertComment(_batchID, _text, _user);

      _eventBus.Publish(new SendNotificationWithPermissionsEvent
      {
        Title = await dbContext.Transalation.GetTranslation(Locale.EN, "MuftiCommentTitle"),
        Body = _text,
        Module = "Mufti",
        RefID = $"{_batchID}",
        Permissions = new List<Permission> {
          Permission.MuftiCommentsRead,
          Permission.MuftiCommentsReadWrite
        },
        Excludes = new string[] { _user.ID.ToString() }
      });

      unitOfWork.Commit();

      return await dbContext.Certificate.GetCommentByID(id);
    }
  }
}
