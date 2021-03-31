using System;
using eHS.Portal.Model;

namespace eHS.Portal.Client
{
  public class PaymentFilter
  {
    public long? ID { get; set; }

    public Guid? CustomerID { get; set; }

    public string Name { get; set; }

    public string TransactionNo { get; set; }

    public PaymentStatus? Status { get; set; }

    public PaymentMode? Mode { get; set; }

    public PaymentMethod? Method { get; set; }

    public DateTimeOffset? From { get; set; }

    public DateTimeOffset? To { get; set; }
  }
}
