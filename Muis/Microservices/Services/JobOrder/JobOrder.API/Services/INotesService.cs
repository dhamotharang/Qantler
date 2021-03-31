using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Model;
using JobOrder.Model;

namespace JobOrder.API.Services
{
  public interface INotesService
  {
    /// <summary>
    /// To add notes.
    /// </summary>
    Task<Notes> AddNotes(Notes notes, Officer user);

    /// <summary>
    /// Retrieve list of notes for specified joborder.
    /// </summary>
    Task<IList<Notes>> ListOfNotes(long jobOrderID);
  }
}
