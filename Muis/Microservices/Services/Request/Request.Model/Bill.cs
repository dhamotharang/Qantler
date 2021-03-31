using System;

namespace Request.Model
{
  public enum BillStatus
  {
    Draft = 100,
    Pending = 200,
    Paid = 300,
    Overdue = 400,
    Canceled = 500
  }

  public enum BillType
  {
    Stage1,
    Stage2
  }
}
