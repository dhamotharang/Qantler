using System;
using System.Collections.Generic;
using System.Text;

namespace eHS.Portal.Model
{
  public class FindingsLineItem
  {
    public long ID { get; set; }

    public Scheme Scheme { get; set; }

    public SubScheme? SubScheme { get; set; }

    public int Index { get; set; }

    public long ChecklistCategoryID { get; set; }

    public string ChecklistCategoryText { get; set; }

    public long ChecklistItemID { get; set; }

    public string ChecklistItemText { get; set; }

    public bool? Complied { get; set; }

    public string Remarks { get; set; }

    public long FindingsID { get; set; }

    public DateTimeOffset? CreatedOn { get; set; }

    public DateTimeOffset? ModifiedOn { get; set; }

    public bool IsDeleted { get; set; }

    #region Generated properties

    public string SchemeText { get; set; }

    public string SubSchemeText { get; set; }

    public IList<Attachment> Attachments { get; set; }

    #endregion
  }
}
