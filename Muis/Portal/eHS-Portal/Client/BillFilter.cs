using eHS.Portal.Model;
using System;

namespace eHS.Portal.Client
{
  public class BillFilter
  {
    public long? ID { get; set; }

    public string InvoiceNo { get; set; }

    public long? RequestID { get; set; }

    public string RefNo { get; set; }

    public string CustomerName { get; set; }

    public string RefID { get; set; }

    public BillStatus? Status { get; set; }

    public DateTimeOffset? From { get; set; }

    public DateTimeOffset? To { get; set; }

    public BillType? Type { get; set; }
  }
}
