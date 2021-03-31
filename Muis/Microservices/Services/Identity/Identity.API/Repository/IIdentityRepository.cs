using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Identity.API.Services;
using Identity.Model;

namespace Identity.API.Repository
{
  public interface IIdentityRepository
  {
    public Task<List<Model.Identity>> GetAllIdentityDetails(long id);

    /// <summary>
    /// Query identity with specified filter.
    /// </summary>
    Task<IList<Model.Identity>> Query(IdentityFilter filter);

    /// <summary>
    /// Insert identity.
    /// </summary>
    Task Insert(Model.Identity identity);

    /// <summary>
    /// Update identity.
    /// </summary>
    Task Update(Model.Identity identity);

    /// <summary>
    /// Maps identity to clusters.
    /// </summary>
    Task MapIdentityToClusters(Guid id, IList<Cluster> clusters);

    /// <summary>
    /// Maps identity to request types.
    /// </summary>
    Task MapIdentityToRequestTypes(Guid id, IList<RequestType> requestTypes);

    /// <summary>
    /// Get identity instance with specified id.
    /// </summary>
    Task<Model.Identity> GetIdentityByID(Guid id);

    /// <summary>
    /// Maps identity to log.
    /// </summary>
    Task MapIdentityToLog(Guid id, long logID);

    /// <summary>
    /// Update identity sequence
    /// </summary>
    /// <param name="identity"></param>
    /// <returns></returns>
    Task UpdateIdentitySequence(Model.Identity identity);
  }
}
