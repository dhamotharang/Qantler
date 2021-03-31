using System;
using Core.Model;

namespace Case.Model
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

  public class Certificate
  {
    public long ID { get; set; }

    public string Number { get; set; }

    public CertificateStatus Status { get; set; }

    public string SerialNo { get; set; }

    public Scheme Scheme { get; set; }

    public SubScheme? SubScheme { get; set; }

    public DateTimeOffset? IssuedOn { get; set; }

    public DateTimeOffset? StartsFrom { get; set; }

    public DateTimeOffset? ExpiresOn { get; set; }

    public DateTimeOffset? SuspendedUntil { get; set; }

    public long PremiseID { get; set; }

    public long CaseID { get; set; }

    public DateTimeOffset? CreatedOn { get; set; }

    public DateTimeOffset? ModifiedOn { get; set; }

    public bool IsDeleted { get; set; }

    #region

    public string SchemeText { get; set; }

    public string SubSchemeText { get; set; }

    #endregion
  }
}
