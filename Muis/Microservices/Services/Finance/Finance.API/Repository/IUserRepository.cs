using System;
using System.Threading.Tasks;
using Core.Model;

namespace Finance.API.Repository
{
  public interface IUserRepository
  {
    Task InsertOrReplace(Officer user);
  }
}
