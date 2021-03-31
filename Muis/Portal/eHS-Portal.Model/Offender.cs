using System;
using System.Collections.Generic;

namespace eHS.Portal.Model
{
  public class Offender
  {
    public Guid ID { get; set; }

    public string Name { get; set; }

    #region Generated properties

    public IList<ContactInfo> ContactInfos { get; set; }

    #endregion
  }
}
