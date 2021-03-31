using HalalLibrary.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HalalLibrary.API.Repository
{
  public interface ICertifyingBodyRepository
  {
    public Task<IEnumerable<CertifyingBody>> Select();

    /// <summary>
    /// get certifying body by name
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public Task<CertifyingBody> GetCertifyingBodyByName(string name);

    public Task<long> InsertCertifyingBody(CertifyingBody data);
  }
}
