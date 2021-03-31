using System;
using System.Threading.Tasks;
using Core.Model;

namespace Finance.API.Services
{
  public interface IUserService
  {
    /// <summary>
    /// Insert or replace officer.
    /// </summary>
    Task InsertOrReplaceOfficer(Officer user);
  }
}
