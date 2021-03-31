using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eHS.Portal.Model;

namespace eHS.Portal.Client
{
  public class CertificateDeliveryOptions
  {
    public string CustomerCode { get; set; }

    public string CustomerName { get; set; }

    public string Premise { get; set; }

    public string Postal { get; set; }

    public DateTimeOffset? IssuedOnFrom { get; set; }

    public DateTimeOffset? IssuedOnTo { get; set; }

    public IList<CertificateDeliveryStatus> Status { get; set; }

    public string SerialNo { get; set; }
  }
}
