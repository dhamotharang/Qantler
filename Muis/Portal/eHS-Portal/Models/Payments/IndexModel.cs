using System;
using System.Collections.Generic;
using eHS.Portal.Model;

namespace eHS.Portal.Models.Payments
{
  public class IndexModel
  {
    public IList<Payment> Dataset { get; set; }
    public PaymentStatus? DefaultStatus { get; set; }
  }
}
