using System;
using System.Collections.Generic;

namespace Case.Model
{
  public enum MasterType
  {
    BreachCategory = 501,
    Offence = 502,
  }

  public class Master
  {
    public Guid ID { get; set; }

    public MasterType Type { get; set; }

    public string Value { get; set; }

    public Guid? ParentID { get; set; }

    public DateTimeOffset? CreatedOn { get; set; }

    public DateTimeOffset? ModifiedOn { get; set; }

    public bool IsDeleted { get; set; }

    #region Generated properties

    public IList<Master> Childrens { get; set; }

    #endregion
  }
}
