using System;
using System.Collections.Generic;
using Core.Model;

namespace Request.Model
{
  public enum RFAStatus
  {
    Draft = 1,
    Open = 100,
    PendingReview = 200,
    Closed = 300,
    Expired = 400
  }

  public class RFA
  {
    public long ID { get; set; }

    public RFAStatus Status { get; set; }

    public Guid RaisedBy { get; set; }

    public string RaisedByName { get; set; }

    public long RequestID { get; set; }

    public DateTimeOffset? CreatedOn { get; set; }

    public DateTimeOffset? DueOn { get; set; }

    public DateTimeOffset? ModifiedOn { get; set; }

    public bool IsDeleted { get; set; }

    #region

    public string StatusText { get; set; }

    public IList<RFALineItem> LineItems { get; set; }

    public IList<Attachment> Attachments { get; set; }

    public IList<Log> Logs { get; set; }

    public Request Request { get; set; }

    #endregion
  }
}
