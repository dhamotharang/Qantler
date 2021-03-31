using System;
using System.Collections.Generic;

namespace eHS.Portal.Model
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

    public IDType? IDType { get; set; }

    public string AltID { get; set; }

    public string Name { get; set; }

    public long? CodeID { get; set; }

    public long? GroupCodeID { get; set; }

    public CustomerStatus Status { get; set; }

    public Guid? ParentID { get; set; }

    #region Generated Properties

    public string StatusText { get; set; }

    public string IDTypeText { get; set; }

    public Code Code { get; set; }

    public Code GroupCode { get; set; }

    public Officer Officer { get; set; }

    public Customer Parent { get; set; }

    public IList<Premise> Premises { get; set; }

    public IList<Certificate> Certificates { get; set; }

    #endregion
  }
}
