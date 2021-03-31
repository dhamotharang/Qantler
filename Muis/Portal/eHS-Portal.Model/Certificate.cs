using System;
using System.Collections.Generic;

namespace eHS.Portal.Model
{

  public enum CertificateDeliveryStatus
  {
    Pending = 100,
    Delivered = 200,
    Returned = 300
  }
  public class Certificate
  {
    public long ID { get; set; }

    public RequestType RequestType { get; set; }

    public CertificateTemplate Template { get; set; }

    public bool IsCertifiedTrueCopy { get; set; }

    public CertificateDeliveryStatus Status { get; set; }

    public string Number { get; set; }

    public Scheme Scheme { get; set; }

    public SubScheme? SubScheme { get; set; }

    public DateTimeOffset? StartsFrom { get; set; }

    public DateTimeOffset? IssuedOn { get; set; }

    public DateTimeOffset? ExpiresOn { get; set; }

    public DateTimeOffset? SuspendedUntil { get; set; }

    public string SerialNo { get; set; }

    public Guid CustomerID { get; set; }

    public long? CodeID { get; set; }

    public string CustomerName { get; set; }

    public long PremiseID { get; set; }

    public long MailingPremiseID { get; set; }

    public string Remarks { get; set; }

    public DateTimeOffset? CreatedOn { get; set; }

    public DateTimeOffset? ModifiedOn { get; set; }

    public bool IsDeleted { get; set; }

    #region Generated Properties

    public string RefID { get; set; }

    public string RequestTypeText { get; set; }

    public string SchemeText { get; set; }

    public string SubSchemeText { get; set; }

    public string StatusText { get; set; }

    public IList<Menu> Menus { get; set; }

    public Code CustomerCode { get; set; }

    public Premise Premise { get; set; }

    public Premise MailingPremise { get; set; }

    #endregion

    #region Module specific properties

    public long RequestID { get; set; }

    public long CaseID { get; set; }

    #endregion
  }
}
