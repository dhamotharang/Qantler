using System;
using System.Threading.Tasks;
using Core.Model;

namespace Request.API.Repository
{
  public interface IUserRepository
  {
    /// <summary>
    /// Insert or replace officer.
    /// </summary>
    Task InsertOrReplace(Officer user);
  }
}
