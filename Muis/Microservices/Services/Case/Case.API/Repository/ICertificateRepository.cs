using Case.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Case.API.Repository
{
  public interface ICertificateRepository
  {
    /// <summary>
    /// insert certificate
    /// </summary>
    Task InsertCertificate(Certificate certificate);

    /// <summary>
    /// update certificate
    /// </summary>
    Task UpdateCertificate(Certificate certificate);

    /// <summary>
    /// get certificate
    /// </summary>
    Task<IList<Certificate>> GetCertificate(long? caseID);
  }
}
