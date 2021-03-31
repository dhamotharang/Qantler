using System;
using System.Linq;
using System.Threading.Tasks;
using Core.API;
using Core.API.Repository;
using Core.Model;
using Request.API.Repository;

namespace Request.API.Services.Commands.Notes
{
  public class AddNotesCommand : IUnitOfWorkCommand<Model.Notes>
  {
    readonly Model.Notes _notes;
    readonly Officer _user;

    public AddNotesCommand(Model.Notes notes, Officer user)
    {
      _notes = notes;
      _user = user;
    }

    public async Task<Model.Notes> Invoke(IUnitOfWork uow)
    {
      var dbContext = DbContext.From(uow);
      uow.BeginTransaction();

      _notes.Officer = _user;
      _notes.CreatedBy = _user.ID;
      _notes.CreatedOn = DateTimeOffset.UtcNow;

      _notes.ID = await dbContext.Notes.InsertNotes(_notes);

      if (_notes.Attachments?.Any() ?? false)
      {
        foreach(var attachment in _notes.Attachments)
        {
          attachment.ID = await dbContext.Attachment.InsertAttachment(attachment);
        }

        await dbContext.Notes.MapNotesAttachments(
          _notes.ID,
          _notes.Attachments.Select(e => e.ID).ToArray());
      }

      uow.Commit();

      return _notes;
    }
  }
}
