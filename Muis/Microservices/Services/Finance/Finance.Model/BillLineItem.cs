using System;

namespace Finance.Model
{
  public class BillLineItem
  {
    public long ID { get; set; }

    public int SectionIndex { get; set; }

    public string Section { get; set; }

    public int Index { get; set; }

    public long CodeID { get; set; }

    public string Code { get; set; }

    public string Descr { get; set; }

    public decimal? Qty { get; set; }

    public decimal? UnitPrice { get; set; }

    public decimal Amount { get; set; }

    public decimal GSTAmount { get; set; }

    public decimal GST { get; set; }

    public bool WillRecord { get; set; }

    public long BillID { get; set; }

    public DateTimeOffset? CreatedOn { get; set; }

    public DateTimeOffset? ModifiedOn { get; set; }

    public bool IsDeleted { get; set; }
  }
}
