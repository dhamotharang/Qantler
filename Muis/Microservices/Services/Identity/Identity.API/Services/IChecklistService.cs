using Core.Model;
using Request.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Identity.API.Services
{
  public interface IChecklistService
  {
    /// <summary>
    /// Get checklist for the specified ID.
    /// </summary>
    /// <param name="id">the reference ID</param>
    /// <returns>the checklist instance</returns>
    Task<ChecklistHistory> GetChecklistHistoryByID(long id);

    /// <summary>
    /// Get latest checklist for the specified scheme.
    /// </summary>
    /// <param name="scheme">the scheme</param>
    Task<ChecklistHistory> GetLatest(Scheme scheme);

    /// <summary>
    /// Get latest checklist for the specified scheme.
    /// </summary>
    /// <param name="id">id</param>
    Task<IEnumerable<ChecklistHistory>> GetChecklistHistoryByScheme(int id);

    /// <summary>
    /// Get latest checklist for the specified scheme.
    /// </summary>
    /// <param name="checklist">the checklist</param>
    Task<bool> InsertChecklist(ChecklistHistory checklist);

    /// <summary>
    /// Get latest checklist for the specified scheme.
    /// </summary>
    /// <param name="checklist">the checklist</param>
    Task<bool> UpdateChecklist(ChecklistHistory checklist);
  }
}
