using System;
using System.Threading.Tasks;
using Core.API;
using Core.API.Exceptions;
using Core.API.Repository;
using Core.Model;
using Identity.API.Repository;
using Request.Model;

namespace Identity.API.Services.Commands.Checklist
{
  public class GetLatestChecklistCommand : IUnitOfWorkCommand<ChecklistHistory>
  {
    readonly Scheme _scheme;

    public GetLatestChecklistCommand(Scheme scheme)
    {
      _scheme = scheme;
    }

    public async Task<ChecklistHistory> Invoke(IUnitOfWork unitOfWork)
    {
      var result = await DbContext.From(unitOfWork).Checklist.GetLatestChecklist(_scheme);
      if (result == null)
      {
        // TODO Provide error message
        throw new NotFoundException();
      }
      return result;
    }
  }
}
