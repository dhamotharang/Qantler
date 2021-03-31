using System;
using System.Linq;
using System.Threading.Tasks;
using Core.API;
using Core.API.Repository;
using Core.Model;
using Finance.API.Repository;
using Finance.Model;

namespace Finance.API.Services.Commands.Payment
{
  public class AddPaymentNoteCommand : IUnitOfWorkCommand<Model.Note>
  {
    readonly Note _note;
    readonly Officer _user;
    readonly long _id;

    public AddPaymentNoteCommand(long id, Note note, Officer user)
    {
      _note = note;
      _user = user;
      _id = id;
    }

    public async Task<Model.Note> Invoke(IUnitOfWork uow)
    {
      var dbContext = DbContext.From(uow);
      uow.BeginTransaction();

      _note.Officer = _user;

      _note.ID = await dbContext.Note.InsertNotes(_note);

      if (_note.Attachments?.Any() ?? false)
      {
        foreach (var attachment in _note.Attachments)
        {
          attachment.ID = await dbContext.Attachment.InsertAttachment(attachment);

          await dbContext.Note.MapAttachment(
          _note.ID,
          attachment.ID);
        }
      }

      await dbContext.Payment.MapNote(_id, _note.ID);

      uow.Commit();

      return _note;
    }
  }
}
