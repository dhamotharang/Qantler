using System;
using System.Collections.Generic;

namespace eHS.Portal.Model
{
  public class ChecklistHistory
  {
    public long ID { get; set; }

    public int Version { get; set; }

    public Scheme Scheme { get; set; }

    public Guid CreatedBy { get; set; }

    public string CreatedByName { get; set; }

    public DateTimeOffset EffectiveFrom { get; set; }

    public DateTimeOffset? CreatedOn { get; set; }

    public DateTimeOffset? ModifiedOn { get; set; }

    public bool IsDeleted { get; set; }

    #region Generated properties

    public IList<ChecklistCategory> Categories { get; set; }

    public string SchemeText { get; set; }

    #endregion
  }

  public class ChecklistCategory
  {
    public long ID { get; set; }

    public int Index { get; set; }

    public string Text { get; set; }

    public long HistoryID { get; set; }

    #region Generated properties

    public IList<ChecklistItem> Items { get; set; }

    #endregion
  }

  public class ChecklistItem
  {
    public long ID { get; set; }

    public int Index { get; set; }

    public string Text { get; set; }

    public string Notes { get; set; }

    public long CategoryID { get; set; }
  }
}
