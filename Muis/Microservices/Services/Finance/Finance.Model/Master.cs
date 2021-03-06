using System;

namespace Finance.Model
{
  public enum MasterType
  {
    Bank = 600
  }

  public class Master
  {
    public MasterType Type { get; set; }

    public Guid ID { get; set; }

    public string Value { get; set; }

    public Guid? ParentID { get; set; }

    public DateTimeOffset? CreatedOn { get; set; }

    public DateTimeOffset? ModifiedOn { get; set; }

    public long IsDeleted { get; set; }

    #region Generated properties

    public Master Parent { get; set; }

    #endregion
  }
}
