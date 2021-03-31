using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.API;
using Core.API.Repository;
using Request.API.Repository;

namespace Request.API.Services.Commands.RFA
{
  public class ListOfRFACommand : IUnitOfWorkCommand<IList<Model.RFA>>
  {
    readonly RFAFilter _filter;

    public ListOfRFACommand(RFAFilter filter)
    {
      _filter = filter;
    }

    public Task<IList<Model.RFA>> Invoke(IUnitOfWork uow)
    {
      return DbContext.From(uow).RFA.Query(_filter);
    }
  }
}
