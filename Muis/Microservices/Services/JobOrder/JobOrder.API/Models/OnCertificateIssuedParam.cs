using Core.Model;
using JobOrder.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobOrder.API.Models
{
  public class OnCertificateIssuedParam
  {
    public Certificate Certificate { get; set; }

    public Guid CustomerID { get; set; }

    public string CustomerName { get; set; }

    public Premise Premise { get; set; }

  }
}

