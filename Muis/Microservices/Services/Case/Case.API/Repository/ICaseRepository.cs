using Case.API.Params;
using Case.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Case.API.Repository
{
  public interface ICaseRepository
  {
    /// <summary>
    /// Select case.
    /// </summary>
    public Task<IEnumerable<Model.Case>> Select(CaseOptions options);

    /// <summary>
    /// Get case by ID.
    /// </summary>
    public Task<Model.Case> GetByID(long id);

    /// <summary>
    /// Get case basics by ID.
    /// </summary>
    public Task<Model.Case> GetBasicByID(long id);

    /// <summary>
    /// Get activity by case.
    /// </summary>
    public Task<IList<Activity>> GetActivityByCaseID(long id);

    /// <summary>
    /// Insert case.
    /// </summary>
    public Task<long> InsertCase(Model.Case data);

    /// <summary>
    /// update case.
    /// </summary>
    public Task<long> UpdateCaseInfo(Model.Case data);

    /// <summary>
    /// Map case and attachment
    /// </summary>
    public Task MapCaseAttachments(long notesID, params long[] attachmentID);

    /// <summary>
    /// Map case and offence
    /// </summary>
    public Task MapCaseOffence(IList<Master> offence, long caseID);

    /// <summary>
    /// Map case and breach
    /// </summary>
    public Task MapCaseBreachByOffence(IList<Master> offence, long caseID);

    /// <summary>
    /// Map case and premise
    /// </summary>
    public Task MapCasePremise(long[] premises, long caseID);

    /// <summary>
    /// Update case status
    /// </summary>
    public Task UpdateStatus(long id, CaseStatus status, CaseMinorStatus? statusMinor = null);

    /// <summary>
    /// Update case and sanction Info
    /// </summary>
    public Task MapCaseSanctionInfo(long caseID, long sanctionInfoID);
  }
}
