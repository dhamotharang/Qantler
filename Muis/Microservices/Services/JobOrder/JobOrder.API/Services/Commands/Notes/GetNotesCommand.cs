using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.API;
using Core.API.Repository;
using JobOrder.API.Repository;

namespace JobOrder.API.Services.Commands.Notes
{
  public class GetNotesCommand : IUnitOfWorkCommand<IList<Model.Notes>>
  {
    readonly long _jobOrderID;

    public GetNotesCommand(long jobOrderID)
    {
      _jobOrderID = jobOrderID;
    }

    public Task<IList<Model.Notes>> Invoke(IUnitOfWork uow)
    {
      return DbContext.From(uow).Notes.SelectNotes(_jobOrderID);
    }
  }
}
