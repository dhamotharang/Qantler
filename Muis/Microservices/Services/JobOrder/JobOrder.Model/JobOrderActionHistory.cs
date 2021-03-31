using System;

namespace JobOrder.Model
{
  public enum JobOrderActionType
  {
    Start = 300,
    Done = 400
  }

  public class JobOrderActionHistory
  {
    public long ID { get; set; }

    public JobOrderActionType Action { get; set; }

    public Guid? OfficerID { get; set; }

    public long JobID { get; set; }

    public string Remarks { get; set; }

    public DateTimeOffset? CreatedOn { get; set; }

    #region Generated Properties

    public string ActionText { get; set; }

    #endregion
  }
}
