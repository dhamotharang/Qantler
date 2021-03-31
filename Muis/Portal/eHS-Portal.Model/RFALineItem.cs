using System;
using System.Collections.Generic;

namespace eHS.Portal.Model
{
  public class RFALineItem
  {
    public long ID { get; set; }

    public Scheme? Scheme { get; set; }

    public int Index { get; set; }

    public long RFAID { get; set; }

    public long ChecklistCategoryID { get; set; }

    public string ChecklistCategoryText { get; set; }

    public long ChecklistID { get; set; }

    public string ChecklistText { get; set; }

    public string Remarks { get; set; }

    public DateTimeOffset? CreatedOn { get; set; }

    public DateTimeOffset? ModifiedOn { get; set; }

    #region Generated properties

    public string SchemeText { get; set; }

    public IList<Attachment> Attachments { get; set; }

    public IList<RFAReply> Replies { get; set; }

    #endregion
  }
}
