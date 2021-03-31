using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Finance.Model;

namespace Finance.API.Strategies.Billing.Billables
{
  public interface IBillable
  {
    Task<IList<BillLineItem>> Generate(BillRequest request);
  }
}
