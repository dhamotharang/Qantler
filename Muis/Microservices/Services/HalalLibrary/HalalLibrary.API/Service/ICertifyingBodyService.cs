using HalalLibrary.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HalalLibrary.API.Services
{
  public interface ICertifyingBodyService
  { 
    /// <summary>
    /// Gets all requets for a particular user.
    /// </summary>
    /// <returns>The list of supplier data.</returns>
    Task<IEnumerable<CertifyingBody>> Select();

    /// <summary>
    /// Insert Certifying Body data.
    /// </summary>
    /// <param name="data">CertifyingBody data</param>
    Task<long> InsertCertifyingBody(CertifyingBody data);
  }
}
