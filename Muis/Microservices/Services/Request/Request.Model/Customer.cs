using Core.Model;
using System;

namespace Request.Model
{
  public class Customer
  {
    public Guid ID { get; set; }

    public string Name { get; set; }

    public IDType? IDType { get; set; }

    public string AltID { get; set; }

    public long? CodeID { get; set; }

    public long? GroupCodeID { get; set; }

    public Guid? OfficerInCharge { get; set; }

    #region Generated properties

    public Code Code { get; set; }

    public Code GroupCode { get; set; }

    public string IDTypeText { get; set; }
    
    public Officer Officer { get; set; }

    #endregion
  }
}
