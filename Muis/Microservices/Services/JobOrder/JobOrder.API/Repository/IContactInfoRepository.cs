using JobOrder.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobOrder.API.Repository
{
  public interface IContactInfoRepository
  {
    /// <summary>
    /// Insert ContactInfo .
    /// </summary>
    public Task<long> InsertContactInfo(ContactInfo info);

    /// <summary>
    /// Get list of contact info for an person
    /// </summary>
    public Task<IList<ContactInfo>> Select(Guid personID, ContactInfoType type);

    /// <summary>
    /// Update ContactInfo
    /// </summary>
    public Task UpdateContactInfo(ContactInfo info);
  }
}
