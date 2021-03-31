using System;
using System.Collections.Generic;
using Core.Model;

namespace JobOrder.Model
{
  public class Findings
  {
    public long ID { get; set; }

    public string Remarks { get; set; }

    public Guid? OfficerID { get; set; }

    public long JobID { get; set; }

    public DateTimeOffset? CreatedOn { get; set; }

    public DateTimeOffset? ModifiedOn { get; set; }

    public bool IsDeleted { get; set; }

    #region Generated properties

    public string OfficerName { get; set; }

    public Officer Officer { get; set; }

    public Attachment Attachment { get; set; }

    public IList<FindingsLineItem> LineItems { get; set; }

    #endregion
  }
}
