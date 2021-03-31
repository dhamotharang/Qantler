using Finance.Model;
using System;
using System.Threading.Tasks;

namespace Finance.API.Repository
{
  public interface IContactInfoRepository
  {
    /// <summary>
    /// Insert ContactInfo .
    /// </summary>
    public Task<long> Insert(ContactInfo info);

    /// <summary>
    /// Update ContactInfo
    /// </summary>
    public Task DeleteByID(long id);
  }
}
