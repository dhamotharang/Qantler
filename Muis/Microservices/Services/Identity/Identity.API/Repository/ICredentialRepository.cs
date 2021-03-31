using System;
using System.Threading.Tasks;
using Identity.Model;

namespace Identity.API.Repository
{
  public interface ICredentialRepository
  {
    /// <summary>
    /// Insert credential.
    /// </summary>
    Task InsertCredential(Credential credential);

    /// <summary>
    /// Update credential.
    /// </summary>
    Task UpdateCredential(Credential credential);

    /// <summary>
    /// Retrieve credential.
    /// </summary>
    Task<Credential> GetCredential(Provider provider, string key, string secret);

    /// <summary>
    /// Retrieve credential by key.
    /// </summary>
    Task<Credential> GetCredentialByKey(Provider provider, string key);
  }
}
