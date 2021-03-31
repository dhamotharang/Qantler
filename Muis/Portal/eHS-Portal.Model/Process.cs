using System;

namespace eHS.Portal.Model
{
  public enum ProcessType
  {
    Info = 100,
    Review = 200,
    JobOrder = 300,
    Approval = 400,
    Billing = 500,
    Payment = 600,
    Issuance = 700
  }

  public class Process
  {
    public long ID { get; set; }

    public ProcessType Type { get; set; }

    public long? RefID { get; set; }

    public int Index { get; set; }

    #region Generated properties

    public string TypeText { get; set; }

    #endregion
  }
}
