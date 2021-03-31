using eHS.Portal.Client;
using eHS.Portal.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eHS.Portal.Services.Checklist
{
  public class ChecklistService : IChecklistService
  {
    readonly ApiClient _apiClient;

    public ChecklistService(ApiClient apiClient)
    {
      _apiClient = apiClient;
    }

    public async Task<ChecklistHistory> GetChecklistHistoryByID(long id)
    {
      return await _apiClient.ChecklistSdk.GetByID(id);
    }

    public async Task<IList<ChecklistHistory>> GetChecklistHistoryByScheme(long id)
    {
      return await _apiClient.ChecklistSdk.GetByScheme(id);
    }

    public async Task<bool> InsertChecklist(ChecklistHistory data)
    {
      return await _apiClient.ChecklistSdk.InsertChecklist(data);
    }

    public async Task<bool> UpdateChecklist(ChecklistHistory data)
    {
      return await _apiClient.ChecklistSdk.UpdateChecklist(data);
    }
  }
}
