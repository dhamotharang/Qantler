using Core.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobOrder.Model
{
  public enum CertitifcateStatus
  {
    Active = 100,
    Cancelled = 200,
    Invalid = 300,
    Expired = 400,
    Suspended = 400
  }
  public class Certificate
  {
    public long ID { get; set; }

    public string Number { get; set; }

    public CertitifcateStatus Status { get; set; }

    public Scheme Scheme { get; set; }

    public SubScheme? SubScheme { get; set; }
    
    public DateTimeOffset? IssuedOn { get; set; }

    public DateTimeOffset? StartsFrom { get; set; }

    public DateTimeOffset? ExpiresOn { get; set; }

    public Guid CustomerID { get; set; }

    public long PremiseID { get; set; }

    public DateTimeOffset? CreatedOn { get; set; }

    public DateTimeOffset? ModifedOn { get; set; }

    public bool IsDeleted { get; set; }

    #region Generated Properties

    public Premise Premise { get; set; }

    public Customer Customer { get; set; }

    #endregion
  }
}
