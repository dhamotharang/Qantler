using System;

namespace Finance.Model
{
  public class Price
  {
    public long ID { get; set; }

    public decimal Amount { get; set; }

    public DateTimeOffset? EffectiveFrom { get; set; }

    public long TransactionCodeID { get; set; }

    public DateTimeOffset? CreatedOn { get; set; }

    public DateTimeOffset? ModifiedOn { get; set; }

    public bool IsDeleted { get; set; }
  }
}
