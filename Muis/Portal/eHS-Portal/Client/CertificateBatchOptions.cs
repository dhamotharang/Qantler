using System;
using System.Collections.Generic;
using eHS.Portal.Model;

namespace eHS.Portal.Client
{
  public class CertificateBatchOptions
  {
    public DateTimeOffset From { get; set; }

    public IList<CertificateBatchStatus> Status { get; set; }
  }
}
