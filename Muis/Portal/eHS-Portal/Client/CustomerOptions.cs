using eHS.Portal.Model;
using System;
using System.Collections.Generic;

namespace eHS.Portal.Client
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

    public IList<CertificateStatus> Status { get; set; }
  }
}
