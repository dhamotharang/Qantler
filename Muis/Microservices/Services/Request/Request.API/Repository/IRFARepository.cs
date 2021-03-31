using Microsoft.AspNetCore.Mvc;
using Request.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Request.API.Repository
{
  public interface IRFARepository
  {
    /// <summary>
    /// Get RFA Request for an ID.
    /// </summary>
    /// <returns>The RFA Request data.</returns>
    /// <param name="Id">RFA Request identifier.</param>
    public Task<Model.RFA> GetRFAByID(long id);

    /// <summary>
    /// Insert new RFA Request.
    /// </summary>
    /// <returns>The ID of newly insert RFA.</returns>
    /// <param name="rfa">RFA model</param>
    public Task<long> InsertRFA(RFA rfa);

    /// <summary>
    /// Update an existing RFA instance.
    /// </summary>
    /// <param name="rfa">RFA model</param>
    public Task UpdateRFA(RFA rfa);

    /// <summary>
    /// Insert RFA response
    /// </summary>
    /// <param name="rfa"></param>
    /// <returns></returns>
    public Task<long> InsertRFAResponse(RFA rfa);

    /// <summary>
    /// Update RFA status
    /// </summary>
    /// <returns>success</returns>
    /// <param name = "id">RFA ID </param>
    /// <param name="rfaStatus">RFAStatus</param>
    public Task UpdateRFAStatus(long id, RFAStatus rfaStatus, Guid userID, string userName);

    /// <summary>
    /// Extend Due Date For RFA
    /// </summary>
    /// <returns>true</returns>
    /// <param name = "id">RFA ID </param>
    /// <param name="notes">some notes</param>
    /// <param name="toDate">To Date</param>
    /// <param name="userID"> userID</param>
    /// <param name="userName">userName</param>
    public Task ExtendDueDate(long id, string notes, DateTimeOffset toDate, Guid userID,
      string userName);

    /// <summary>
    /// Delete a RFA
    /// </summary>
    /// <returns>bool</returns>
    /// <param name = "id">RFA ID </param>
    Task Delete(long id);

    /// <summary>
    /// Query RFA based on specified filters.
    /// </summary>
    Task<IList<RFA>> Query(RFAFilter filter);
  }

  public class RFAFilter
  {
    public long? ID { get; set; }

    public string Customer { get; set; }

    public long? RequestID { get; set; }

    public IList<RFAStatus> Status { get; set; }

    public Guid? RaisedBy { get; set; }

    public DateTimeOffset? CreatedOn { get; set; }

    public DateTimeOffset? DueOn { get; set; }
  }
}
