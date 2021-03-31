using Core.API;
using Core.API.Provider;
using Core.Model;
using Identity.API.Services.Commands.Checklist;
using Request.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Identity.API.Services
{
  public class ChecklistService : TransactionalService,
                                  IChecklistService
  {
    public ChecklistService(IDbConnectionProvider connectionProvider)
         : base(connectionProvider)
    {
    }

    public async Task<ChecklistHistory> GetChecklistHistoryByID(long id)
    {
      return await Execute(new GetChecklistHistoryByIDCommand(id));
    }

    public async Task<ChecklistHistory> GetLatest(Scheme scheme)
    {
      return await Execute(new GetLatestChecklistCommand(scheme));
    }

    public async Task<IEnumerable<ChecklistHistory>> GetChecklistHistoryByScheme(int id)
    {
      return await Execute(new GetChecklistHistoryBySchemeCommand(id));
    }

    public async Task<bool> InsertChecklist(ChecklistHistory checklist)
    {
      return await Execute(new InsertChecklistCommand(checklist));
    }

    public async Task<bool> UpdateChecklist(ChecklistHistory checklist)
    {
      return await Execute(new UpdateChecklistCommand(checklist));
    }
  }
}
