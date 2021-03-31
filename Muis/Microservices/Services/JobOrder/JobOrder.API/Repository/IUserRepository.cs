using System;
using System.Threading.Tasks;
using Core.Model;

namespace JobOrder.API.Repository
{
  public interface IUserRepository
  {
    /// <summary>
    /// Insert or replace officer.
    /// </summary>
    Task InsertOrReplace(Officer user);

    /// <summary>
    /// Retrieve user instance based on specified id.
    /// </summary>
    Task<Officer> GetUserByID(Guid id);
  }
}
