using Core.Model;
using Request.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Identity.API.Repository
{
  public interface IChecklistRepository
  {
    Task<ChecklistHistory> GetChecklistHistoryByID(long id);

    Task<ChecklistHistory> GetLatestChecklist(Scheme scheme);

    Task<IEnumerable<ChecklistHistory>> GetChecklistHistoryByScheme(int id);

    Task<bool> InsertChecklist(ChecklistHistory checklist);

    Task<bool> UpdateChecklist(ChecklistHistory checklist);
  }
}
