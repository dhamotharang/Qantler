using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Model;
using Identity.Model;

namespace Identity.API.Services
{
  public interface IIdentityService
  {
    /// <summary>
    /// Retrieve identity based on specified id.
    /// </summary>
    Task<Model.Identity> GetIdentityByID(Guid id);

    /// <summary>
    /// List of identities with specified parameters.
    /// </summary>
    Task<IList<Model.Identity>> List(IdentityFilter filter);

    /// <summary>
    /// To create identity.
    /// </summary>
    Task<Model.Identity> CreateIdentity(Model.Identity identity, Officer user);

    /// <summary>
    /// Updates an existing identity.
    /// </summary>
    Task<Model.Identity> UpdateIdentity(Model.Identity identity, Officer user);

    /// <summary>
    /// Reset identity password.
    /// </summary>
    Task ResetPassword(Guid id, Officer user);

    /// <summary>
    /// Forgot identity password.
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    Task ForgotPassword(string email);
    /// <summary>
    /// Get certificate auditor to assign the application requests 
    /// </summary>
    /// <param name="filter"></param>
    /// <param name="ClusterNode"></param>
    /// <returns></returns>
    Task<Model.Identity> GetCertificateAuditorToAssign(IdentityFilter filter,
      string ClusterNode);
  }

  public class IdentityFilter
  {
    public string Name { get; set; }

    public string Email { get; set; }

    public IdentityStatus Status { get; set; }

    public Guid[] IDs { get; set; }

    public RequestType[] RequestTypes { get; set; }

    public Permission[] Permissions { get; set; }

    public long[] Clusters { get; set; }

    public string[] Nodes { get; set; }
  }
}
