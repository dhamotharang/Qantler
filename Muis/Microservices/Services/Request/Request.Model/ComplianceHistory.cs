using System;
using System.Collections.Generic;
using Core.Model;

namespace Request.Model
{
  public class ComplianceHistory
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

    public IList<ComplianceCategory> Categories { get; set; }

    public string SchemeText { get; set; }

    #endregion
  }

  public class ComplianceCategory
  {
    public long ID { get; set; }

    public int Index { get; set; }

    public string Text { get; set; }

    public long HistoryID { get; set; }

    #region Generated properties

    public IList<ComplianceItem> Items { get; set; }

    #endregion
  }

  public class ComplianceItem
  {
    public long ID { get; set; }

    public int Index { get; set; }

    public string Text { get; set; }

    public long CategoryID { get; set; }
  }
}
