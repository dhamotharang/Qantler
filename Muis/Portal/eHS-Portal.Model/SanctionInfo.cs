using System;

namespace eHS.Portal.Model
{
  public enum SanctionInfoType
  {
    Recommended,
    Final
  }

  public class SanctionInfo
  {
    public long ID { get; set; }

    public SanctionInfoType Type { get; set; }

    public Sanction Sanction { get; set; }

    public string Value { get; set; }

    public long CaseID { get; set; }

    public DateTimeOffset? CreatedOn { get; set; }

    public DateTimeOffset? ModifiedOn { get; set; }

    public bool IsDeleted { get; set; }

    #region Generated properties

    public string SanctionText { get; set; }

    public string SanctionInfoTypeText { get; set; }

    #endregion
  }
}
