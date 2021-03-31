using System;
using System.Collections.Generic;
using Core.Model;

namespace Identity.Model
{
  public enum IdentityStatus
  {
    Active,
    InActive,
    Suspended
  }

  public enum Role
  {
    CertificateAuditor = 100,
    ApprovingOfficer = 200,
    FinanceOfficer = 300,
    IssuanceOfficer = 400,
    PeriodicInspector = 500,
    HOD = 600,
    Mufti = 700,
    Admin = 800
  }

  public class Identity
  {
    public Guid ID { get; set; }

    public string Name { get; set; }

    public string Email { get; set; }

    public string Designation { get; set; }

    public Role Role { get; set; }

    public string Permissions { get; set; }

    public IdentityStatus Status { get; set; }

    public int AccessFailCount { get; set; }

    public bool LockoutEnabled { get; set; }

    public DateTimeOffset? LockoutEndOn { get; set; }

    public int? Sequence { get; set; }

    public DateTimeOffset? CreatedOn { get; set; }

    public DateTimeOffset? ModifiedOn { get; set; }

    public bool IsDeleted { get; set; }

    #region Generated properties

    public string StatusText { get; set; }

    public IList<RequestType> RequestTypes { get; set; }

    public IList<Cluster> Clusters { get; set; }

    public IList<Log> Logs { get; set; }

    #endregion
  }
}
