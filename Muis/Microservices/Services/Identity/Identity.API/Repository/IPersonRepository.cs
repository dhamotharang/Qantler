using Identity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.API.Repository
{
  public interface IPersonRepository
  {
    /// <summary>
    /// Get Person for an ID.
    /// </summary>
    /// <returns>The person data.</returns>
    /// <param name="altID">person identifier.</param>
    public Task<Person> GetPersonByAltID(string altID);


    /// <summary>
    /// Insert Person .
    /// </summary>
    /// <returns>The ID of newly inserted person </returns>
    /// <param name="Id">person identifier.</param>
    /// <param name="person"> Person</param>
    public Task<bool> InsertPerson(Person person);

    /// <summary>
    /// Update Person Details
    /// </summary>
    /// <param name="person"></param>
    /// <returns></returns>
    public Task<bool> UpdatePerson(Person person);


    /// <summary>
    /// Validate Person status
    /// </summary>
    /// <returns>true or false</returns>
    /// <param name = "personID">Person ID </param>
    public bool ValidatePersonStatus(string altID);


    /// <summary>
    /// Get Person for an ID.
    /// </summary>
    /// <returns>The person data.</returns>
    /// <param name="ID">Person identifier.</param>
    public Task<Person> GetPersonByID(Guid id);
  }
}
