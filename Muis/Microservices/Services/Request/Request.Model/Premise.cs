using System;
using Core.Model;

namespace Request.Model
{
  public class Premise : BasePremise
  {
    public ChangeType ChangeType { get; set; }

    public long RequestID { get; set; }

    public Guid? CustomerID { get; set; }

    #region Generated properties

    public Customer Customer { get; set; }

    #endregion

    public bool FirstOrDefault(object p)
    {
      throw new NotImplementedException();
    }
  }
}
