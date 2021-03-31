using Request.Model;
using System;

namespace Request.API.Models
{
  public class CustomerOptions
  {
    public Guid? ID { get; set; }

    public string Code { get; set; }

    public string GroupCode { get; set; }

    public string Name { get; set; }

    public string CertificateNo { get; set; }

    public long? PremiseID { get; set; }

    public string Premise { get; set; }

    public CertificateStatus[] Status { get; set; }
  }
}
