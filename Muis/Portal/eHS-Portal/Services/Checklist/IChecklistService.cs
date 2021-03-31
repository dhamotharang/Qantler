using eHS.Portal.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eHS.Portal.Services.Checklist
{
  public interface IChecklistService
  {
    Task<ChecklistHistory> GetChecklistHistoryByID(long id);

    Task<IList<ChecklistHistory>> GetChecklistHistoryByScheme(long id);

    Task<bool> InsertChecklist(ChecklistHistory data);

    Task<bool> UpdateChecklist(ChecklistHistory data);
  }
}
