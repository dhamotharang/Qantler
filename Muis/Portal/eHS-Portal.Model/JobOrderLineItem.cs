using System;

namespace eHS.Portal.Model
{
  public class JobOrderLineItem
  {
    public long ID { get; set; }

    public Scheme Scheme { get; set; }

    public SubScheme? SubScheme { get; set; }

    public long ChecklistHistoryID { get; set; }

    public long JobID { get; set; }

    #region Generated properties

    public string SchemeText { get; set; }

    public string SubSchemeText { get; set; }

    public ChecklistHistory ChecklistHistory { get; set; }

    #endregion
  }
}
