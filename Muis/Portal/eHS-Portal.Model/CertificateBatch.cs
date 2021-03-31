using System;
using System.Collections.Generic;

namespace eHS.Portal.Model
{
  public enum CertificateBatchStatus
  {
    Pending = 100,
    Acknowledged = 200,
    Downloaded = 300,
    SentToCourier = 400,
    Delivered = 500
  }

  public class CertificateBatch
  {
    public long ID { get; set; }

    public string Code { get; set; }

    public string Description { get; set; }

    public CertificateTemplate Template { get; set; }

    public CertificateBatchStatus Status { get; set; }

    public Guid? FileID { get; set; }

    public DateTimeOffset? AcknowledgedOn { get; set; }

    public DateTimeOffset? LastAction { get; set; }

    public DateTimeOffset? CreatedOn { get; set; }

    public DateTimeOffset? ModifiedOn { get; set; }

    public bool IsDeleted { get; set; }

    #region Generated properties

    public string StatusText { get; set; }

    public string TemplateText { get; set; }

    public IList<Certificate> Certificates { get; set; }

    public IList<Comment> Comments { get; set; }

    #endregion
  }
}
