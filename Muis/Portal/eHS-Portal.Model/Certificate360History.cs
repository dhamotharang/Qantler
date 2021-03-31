using System;
using System.Collections.Generic;
using System.Text;

namespace eHS.Portal.Model
{
  public class Certificate360History
  {
    public long ID { get; set; }

    public long RequestID { get; set; }

    public string RefID { get; set; }

    public Guid RequestorID { get; set; }

    public string RequestorName { get; set; }

    public Guid? AgentID { get; set; }

    public string AgentName { get; set; }

    public int Duration { get; set; }

    public DateTimeOffset? IssuedOn { get; set; }

    public DateTimeOffset? ExpiresOn { get; set; }

    public string SerialNo { get; set; }

    public Guid? ApprovedBy { get; set; }

    public DateTimeOffset? ApprovedOn { get; set; }

    public long CertificateID { get; set; }

    public DateTimeOffset? CreatedOn { get; set; }

    public DateTimeOffset? ModifiedOn { get; set; }

    public bool IsDeleted { get; set; }

    #region Generated properties

    public Officer ApprovedByOfficer { get; set; }

    #endregion
  }
}
