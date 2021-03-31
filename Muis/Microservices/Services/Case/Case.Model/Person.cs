using System;
using System.Collections.Generic;

namespace Case.Model
{
  public class Person
  {
    public Guid ID { get; set; }

    public string Name { get; set; }

    public DateTimeOffset CreatedOn { get; set; }

    public DateTimeOffset ModifiedOn { get; set; }

    public bool IsDeleted { get; set; }

    #region

    public IList<ContactInfo> ContactInfos { get; set; }

    #endregion
  }
}
