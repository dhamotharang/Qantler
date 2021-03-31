using System;
using System.Collections.Generic;
using Core.Model;

namespace Identity.Model
{
  public class Premise : BasePremise
  {
    public Guid CustomerID { get; set; }

    #region Generated properties

    public Customer Customer { get; set; }

    #endregion
  }
}
