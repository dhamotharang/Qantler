using System;
using System.Collections.Generic;
using Core.Model;

namespace Finance.Model
{
  public class Person
  {
    public Guid ID { get; set; }

    public string Name { get; set; }

    public DateTimeOffset CreatedOn { get; set; }

    public DateTimeOffset ModifiedOn { get; set; }

    public bool IsDeleted { get; set; }

    #region Generated properties

    public IList<ContactInfo> ContactInfos { get; set; }

    #endregion
  }
}
