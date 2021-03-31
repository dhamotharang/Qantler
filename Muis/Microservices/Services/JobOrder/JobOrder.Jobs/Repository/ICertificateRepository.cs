using JobOrder.Jobs.Model;
using JobOrder.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JobOrder.Jobs.Repository
{
  public interface ICertificateRepository
  {
    /// <summary>
    /// Get certificates instances
    /// </summary>
    public Task<Certificate> Select(CertificateOptions certificate);
  }
}
