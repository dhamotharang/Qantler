using System;
using System.Collections.Generic;
using System.Text;

namespace JobOrder.Jobs.Model
{
  public class CertificateOptions
  {
    public string? Number { get; set; }

    public int? Status { get; set; }

    public int? Scheme { get; set; }

    public int? SubScheme { get; set; }

    public Guid? CustomerID { get; set; }

    public long? PremiseID { get; set; }
  }
}
