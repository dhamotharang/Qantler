using Core.Model;
using JobOrder.API.Models;
using JobOrder.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobOrder.API.Repository
{
  public interface ICertificateRepository
  {
    /// <summary>
    /// Retrieve certificate instances based on number, premise, scheme, subscheme, customer,
    /// status
    /// </summary>
    public Task<IEnumerable<Certificate>> Select(CertificateOptions options);
    /// <summary>
    /// Insert or replace certificate.
    /// </summary>
    Task InsertOrReplace(Certificate certificate);

    /// <summary>
    /// Delete certificate instance based on specified premise with same scheme and subscheme.
    /// </summary>
    Task DeleteCertificateByPremise(long premiseID, Scheme schme, SubScheme? subScheme);
  }
}
