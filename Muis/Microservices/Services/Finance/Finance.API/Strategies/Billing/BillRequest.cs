using System;
using System.Collections.Generic;
using Core.Model;
using Finance.Model;

namespace Finance.API.Strategies.Billing
{
  public class BillRequest
  {
    public string RefNo { get; set; }

    public long RequestID { get; set; }

    public string RefID { get; set; }

    public BillType Type { get; set; }

    public BillRequestType RequestType { get; set; }

    public bool Expedite { get; set; }

    public Guid CustomerID { get; set; }

    public string CustomerName { get; set; }

    public IList<BillRequestLineItem> LineItems { get; set; }

    public DateTimeOffset ReferenceDate { get; set; }
  }

  public class BillRequestLineItem
  {
    public Scheme Scheme { get; set; }

    public SubScheme? SubScheme { get; set; }

    public int NoOfProducts { get; set; }

    public float Area { get; set; }

    public DateTimeOffset StartsFrom { get; set; }

    public DateTimeOffset ExpiresOn { get; set; }
  }
}
