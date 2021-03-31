using Core.API;
using Core.API.Repository;
using Identity.API.Repository;
using Request.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Identity.API.Services.Commands.Checklist
{
  public class GetChecklistHistoryBySchemeCommand : IUnitOfWorkCommand<IEnumerable<ChecklistHistory>>
  {
    readonly int _id;

    public GetChecklistHistoryBySchemeCommand(int id)
    {
      _id = id;
    }

    public async Task<IEnumerable<ChecklistHistory>> Invoke(IUnitOfWork unitOfWork)
    {
      return await DbContext.From(unitOfWork).Checklist.GetChecklistHistoryByScheme(_id);
    }
  }
}
