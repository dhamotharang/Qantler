using System;
using System.Collections.Generic;
using Core.Model;

namespace Request.Model
{
  public class Review
  {
    public long ID { get; set; }

    public long? RefID { get; set; }

    public int? Step { get; set; }

    public int? Grade { get; set; }

    public Guid? ReviewerID { get; set; }

    public string ReviewerName { get; set; }

    public long? EmailID { get; set; }

    public long RequestID { get; set; }

    public DateTimeOffset? CreatedOn { get; set; }

    #region Generated Properties

    public IList<Attachment> Attachments { get; set; }

    public IList<ReviewLineItem> LineItems { get; set; }

    public Email Email { get; set; }

    #endregion
  }
}
