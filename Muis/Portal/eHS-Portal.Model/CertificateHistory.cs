using System;

namespace eHS.Portal.Model
{
  public class CertificateHistory
  {
    public long ID { get; set; }

    public long RequestID { get; set; }

    public string RefID { get; set; }

    public DateTimeOffset IssuedOn { get; set; }

    public DateTimeOffset ExpiresOn { get; set; }

    public string SerialNo { get; set; }

    public Guid ApprovedBy { get; set; }

    public DateTimeOffset ApprovedOn { get; set; }

    public string ApprovedByName { get; set; }

    public long CertificateID { get; set; }

    public DateTimeOffset CreatedOn { get; set; }

    public DateTimeOffset ModifiedOn { get; set; }

    public bool IsDeleted { get; set; }
  }
}
