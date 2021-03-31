using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Finance.Model;

namespace Finance.API.Repository
{
  public interface INotesRepository
  {
    /// <summary>
    /// To insert notes entity.
    /// </summary>
    Task<long> InsertNotes(Note entity);

    /// <summary>
    /// Maps attachments to notes.
    /// </summary>
    Task MapAttachment(long notesID, long attachmentID);
  }
}
