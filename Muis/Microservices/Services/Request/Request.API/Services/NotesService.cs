using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.API;
using Core.API.Provider;
using Core.Model;
using Request.API.Services.Commands.Notes;
using Request.Model;

namespace Request.API.Services
{
  public class NotesService : TransactionalService,
                              INotesService
  {
    public NotesService(IDbConnectionProvider connectionProvider)
         : base(connectionProvider)
    {
    }

    public Task<Notes> AddNotes(Notes notes, Officer user)
    {
      return Execute(new AddNotesCommand(notes, user));
    }

    public Task<IList<Notes>> ListOfNotes(long requestID)
    {
      return Execute(new GetNotesCommand(requestID));
    }
  }
}
