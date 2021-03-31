using Core.Model;
using JobOrder.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobOrder.API.Models
{
  public class CertificateOptions
  {
    public long? ID { get; set; }

    public string? Number { get; set; }

    public CertitifcateStatus? Status { get; set; }

    public Scheme? Scheme { get; set; }

    public SubScheme? SubScheme { get; set; }

    public Guid? CustomerID { get; set; }

    public long? PremiseID { get; set; }

    public bool? IsDeleted { get; set; }
  }
}
