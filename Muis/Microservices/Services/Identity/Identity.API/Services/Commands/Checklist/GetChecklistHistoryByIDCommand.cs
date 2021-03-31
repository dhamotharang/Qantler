using System;
using System.Threading.Tasks;
using Core.API;
using Core.API.Repository;
using Identity.API.Repository;
using Request.Model;

namespace Identity.API.Services.Commands.Checklist
{
  public class GetChecklistHistoryByIDCommand : IUnitOfWorkCommand<ChecklistHistory>
  {
    readonly long _id;

    public GetChecklistHistoryByIDCommand(long id)
    {
      _id = id;
    }

    public async Task<ChecklistHistory> Invoke(IUnitOfWork unitOfWork)
    {
      return await DbContext.From(unitOfWork).Checklist.GetChecklistHistoryByID(_id);
    }
  }
}
