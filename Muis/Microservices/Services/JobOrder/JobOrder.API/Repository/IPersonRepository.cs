using JobOrder.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobOrder.API.Repository
{
  public interface IPersonRepository
  {
    /// <summary>
    /// Insert Person .
    /// </summary>
    public Task InsertPerson(Person person);

    /// <summary>
    /// Get Person for an ID.
    /// </summary>
    public Task<Person> GetPersonByID(Guid id);

    /// <summary>
    /// Map contact info to person
    /// </summary>
    public Task MapContactInfoToPerson(Guid personID, long contactID);
  }
}
