using Finance.Model;
using System;
using System.Threading.Tasks;

namespace Finance.API.Repository
{
  public interface IPersonRepository
  {
    /// <summary>
    /// Insert Person .
    /// </summary>
    public Task Insert(Person entity);

    /// <summary>
    /// Update Person .
    /// </summary>
    public Task Update(Person entity);

    /// <summary>
    /// Get Person for an ID.
    /// </summary>
    public Task<Person> GetByID(Guid id);

    /// <summary>
    /// Map contact info to person
    /// </summary>
    public Task MapContactInfo(Guid personID, long contactID);

    /// <summary>
    /// Remove contact info from specified person.
    /// </summary>
    Task RemoveContactInfo(Guid personID, long contactID);
  }
}
