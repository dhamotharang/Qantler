using System;
using System.Collections.Generic;
using Core.Model;

namespace Request.Model
{
  public class RFAReply
  {
    public long ID { get; set; }

    public string Text { get; set; }

    public long LineItemID { get; set; }

    public DateTimeOffset? RepliedOn { get; set; }

    public DateTimeOffset? CreatedOn { get; set; }

    public DateTimeOffset? ModifiedOn { get; set; }

    public bool IsDeleted { get; set; }

    #region Generated properties

    public IList<Attachment> Attachments { get; set; }

    #endregion
  }
}
