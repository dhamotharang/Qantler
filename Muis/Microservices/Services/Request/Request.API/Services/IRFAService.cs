using Request.API.Repository;
using Request.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Request.API.Services
{
  public interface IRFAService
  {
    /// <summary>
    /// Gets RFA Request detail by ID.
    /// </summary>
    /// <returns>The RFA Request data.</returns>
    /// <param name="id">RFA identifier.</param>
    Task<RFA> GetRFAByID(long id);

    /// <summary>
    /// Submit new RFA Request.
    /// </summary>
    /// <returns>The RFA Request created </returns>
    /// <param name="rfa">RFA Request.</param>
    Task<RFA> SubmitRFA(RFA rfa);

    /// <summary>
    /// Submit RFA Response.
    /// </summary>
    /// <returns>The RFA Request created </returns>
    /// <param name="rfa">RFA Request.</param>
    Task<RFA> SubmitRFAResponse(RFA rfa);

    /// <summary>
    /// Update RFA status
    /// </summary>
    /// <returns>success</returns>
    /// <param name = "id">RFA ID </param>
    /// <param name="rfaStatus">RFAStatus</param>
    Task UpdateRFAStatus(long id, RFAStatus rfaStatus, Guid userID, string userName);

    /// <summary>
    /// Extend RFA due date.
    /// </summary>
    /// <param name="id">the reference ID</param>
    /// <param name="notes">some notes</param>
    /// <param name="toDate">to date</param>
    /// <param name="userID">the user ID</param>
    /// <param name="userName">the user Name</param>
    Task ExtendDueDate(long id, string notes, DateTimeOffset toDate, Guid userID,
      string userName);

    /// <summary>
    /// Delete a RFA
    /// </summary>
    /// <returns>bool</returns>
    /// <param name = "id">RFA ID </param>
    Task<bool> Delete(long id);

    /// <summary>
    /// Retrieve list of RFA based on specified filter.
    /// </summary>
    Task<IList<RFA>> ListOfRFA(RFAFilter filter);
  }
}
