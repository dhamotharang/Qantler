using System;
using System.Collections.Generic;
using System.Text;

namespace eHS.Portal.Model
{
  public enum CertificateStatus
  {
    Active = 100,
    Cancelled = 200,
    Invalid = 300,
    Expired = 400,
    Suspended = 500,
    Revoked = 600
  }

  public class Certificate360
  {
    public long ID { get; set; }

    public string Number { get; set; }

    public CertificateStatus Status { get; set; }

    public CertificateTemplate Template { get; set; }

    public Scheme Scheme { get; set; }

    public SubScheme? SubScheme { get; set; }

    public DateTimeOffset? IssuedOn { get; set; }

    public DateTimeOffset? ExpiresOn { get; set; }

    public DateTimeOffset? SuspendedUntil { get; set; }

    public string SerialNo { get; set; }

    public Guid CustomerID { get; set; }

    public string CustomerName { get; set; }

    public Guid RequestorID { get; set; }

    public string RequestorName { get; set; }

    public Guid? AgentID { get; set; }

    public string AgentName { get; set; }

    public long PremiseID { get; set; }

    public DateTimeOffset? CreatedOn { get; set; }

    public DateTimeOffset? ModifiedOn { get; set; }

    public bool IsDeleted { get; set; }

    #region Generated Properties

    public string StatusText { get; set; }

    public string SchemeText { get; set; }

    public string SubSchemeText { get; set; }

    public IList<HalalTeam> Teams { get; set; }

    public IList<Menu> Menus { get; set; }

    public IList<Ingredient> Ingredients { get; set; }

    public Premise Premise { get; set; }

    public Customer Customer { get; set; }

    public IList<Certificate360History> History { get; set; }

    #endregion
  }
}
