using eHS.Portal.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eHS.Portal.Services
{
  public interface IIdentityService
  {
    /// <summary>
    /// Retrieve identity based on specified id.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<Identity> GetIdentityByID(Guid id);

    /// <summary>
    /// List of identities with specified parameters.
    /// </summary>
    Task<IList<Identity>> List(IdentityFilter filter);

    /// <summary>
    /// Create user.
    /// </summary>
    Task<Identity> CreateUser(Identity identity);

    /// <summary>
    /// Update user.
    /// </summary>
    Task<Identity> UpdateUser(Identity identity);

    /// <summary>
    /// Reset password for specified identity.
    /// </summary>
    Task ResetPassword(Guid id);

    /// <summary>
    /// Forgot password for specified identity.
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    Task ForgotPassword(string email);
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