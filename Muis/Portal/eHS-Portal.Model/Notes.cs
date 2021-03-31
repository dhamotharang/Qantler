using System;
using System.Collections.Generic;

namespace eHS.Portal.Model
{
  public class Notes
  {
    public long ID { get; set; }

    public string Text { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTimeOffset? CreatedOn { get; set; }

    public DateTimeOffset? ModifiedOn { get; set; }

    public bool IsDeleted { get; set; }

    #region Generated properties

    public Officer Officer { get; set; }

    public IList<Attachment> Attachments { get; set; }

    #endregion

    #region Module specific properties

    public long? RequestID { get; set; }

    public long? JobOrderID { get; set; }

    #endregion
  }
}
