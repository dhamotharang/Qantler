using JobOrder.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobOrder.Jobs.Repository.Mappers
{
  public class CertificateMapper
  {
    readonly Dictionary<long, Certificate> _certificateCache =
      new Dictionary<long, Certificate>();

    public Certificate Map(Certificate certificate, Premise premise, Customer customer)
    {
      if (!_certificateCache.TryGetValue(certificate.ID, out Certificate result))
      {
        _certificateCache[certificate.ID] = certificate;
        result = certificate;
      }

      if (premise.ID != 0)
      {
        result.Premise = premise;
      }

      if (customer?.ID != null)
      {
        result.Customer = customer;
      }

      return result;
    }
  }
}
