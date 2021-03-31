using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.API;
using Core.API.Repository;
using Request.API.Repository;

namespace Request.API.Services.Commands.Notes
{
  public class GetNotesCommand : IUnitOfWorkCommand<IList<Model.Notes>>
  {
    readonly long _requestID;

    public GetNotesCommand(long requestID)
    {
      _requestID = requestID;
    }

    public Task<IList<Model.Notes>> Invoke(IUnitOfWork uow)
    {
      return DbContext.From(uow).Notes.SelectNotes(_requestID);
    }
  }
}
