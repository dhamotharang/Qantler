using Identity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.API.Services
{
  public interface IPersonService
  {
    /// <summary>
    /// Get Person for an altID.
    /// </summary>
    /// <returns>The person data.</returns>
    /// <param name="altID">Person identifier.</param>
    Task<Person> GetPersonByAltID(string altID);

    /// <summary>
    /// Insert new Person.
    /// </summary>
    /// <returns>The Person created </returns>
    /// <param name="person"> Person.</param>
    Task<Person> InsertPerson(Person person);

    Task<Person> UpdatePerson(Person person);

    /// Get person by id.
    /// </summary>
    /// <returns>Person Model </returns>
    /// <param name="id">person id</param>       
    Task<Model.Person> GetPersonByID(Guid id);
  }
}
