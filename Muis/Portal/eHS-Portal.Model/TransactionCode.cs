using System;
using System.Collections.Generic;

namespace eHS.Portal.Model
{
  public class TransactionCode
  {
    public long ID { get; set; }

    public string Code { get; set; }

    public string GLEntry { get; set; }

    public string Text { get; set; }

    public string Currency { get; set; }

    public bool IsBillable { get; set; }

    public DateTimeOffset? CreatedOn { get; set; }

    public DateTimeOffset? ModifiedOn { get; set; }

    public bool IsDeleted { get; set; }

    #region Generated properties

    public IList<Price> PriceHistory { get; set; }

    public IList<Condition> Conditions { get; set; }

    public IList<Log> Logs { get; set; }

    #endregion
  }
}
