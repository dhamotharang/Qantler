using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Request.Model;

namespace Request.API.Repository
{
  public interface INotesRepository
  {
    /// <summary>
    /// To insert notes entity.
    /// </summary>
    Task<long> InsertNotes(Notes entity);

    /// <summary>
    /// Select notes with specified params.
    /// </summary>
    Task<IList<Notes>> SelectNotes(long requestID);

    /// <summary>
    /// Maps attachments to notes.
    /// </summary>
    Task MapNotesAttachments(long notesID, params long[] attachmentID);
  }
}
