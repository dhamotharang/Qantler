using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Request.Model;

namespace Request.API.Repository
{
  public interface ICodeRepository
  {
    /// <summary>
    /// Select list of code instances with specified parameters.
    /// </summary>
    Task<IList<Code>> Select(CodeType type);

    /// <summary>
    /// Insert new code.
    /// </summary>
    Task<long> Insert(Code entity);

    /// <summary>
    /// Update code instance.
    /// </summary>
    Task Update(Code entity);

    /// <summary>
    /// Get code instance with specified ID.
    /// </summary>
    Task<Code> GetByID(long id);

    /// <summary>
    /// Generate code series for specified code type.
    /// </summary>
    Task<int> GenerateCodeSeries(CodeType type);

    /// <summary>
    /// Syncs code to all pending request that is associated with the specified customer.
    /// </summary>
    Task SyncCodeToRequest(Guid customerID, long? codeID, long? groupCodeID);

    /// <summary>
    /// Syncs officerInCharge to all pending request that is associated with the specified customer.
    /// </summary>
    /// <param name="customerID"></param>
    /// <param name="officerInCharge"></param>
    /// <returns></returns>
    Task SyncOfficerToRequest(Guid customerID, Guid? officerInCharge);
  }
}
