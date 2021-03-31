using System;
using System.Collections.Generic;
using Core.Model;

namespace Identity.Model
{
  public enum CustomerStatus
  {
    Active,
    Certified,
    Expired,
    InActive
  }

  public class Customer
  {
    public Guid ID { get; set; }

    public IDType IDType { get; set; }

    public string AltID { get; set; }

    public string Name { get; set; }

    public CustomerStatus Status { get; set; }

    public Guid? ParentID { get; set; }

    #region Generated Properties

    public string StatusText { get; set; }

    public string IDTypeText { get; set; }

    public Customer Parent { get; set; }

    #endregion
  }
}
