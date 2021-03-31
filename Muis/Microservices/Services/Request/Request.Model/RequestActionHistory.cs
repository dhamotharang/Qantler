using System;
using Core.Model;

namespace Request.Model
{
  public enum RequestActionType
  {
    Review = 1,
    SiteInspection,
    Approved,
    Invoice,
    ReceivePartialPayment,
    ReceiveFullPayment,
    CertificateIssuance,
    Reassign,
    Reaudit,
    Reinstate,
    ReviewApproval
  }

  public class RequestActionHistory
  {
    public long ID { get; set; }

    public RequestActionType Action { get; set; }

    public Guid OfficerID { get; set; }

    public long RequestID { get; set; }

    public string RefID { get; set; }

    public string Remarks { get; set; }

    public DateTimeOffset CreatedOn { get; set; }

    #region Generated properties

    public string ActionText { get; set; }

    public Officer Officer { get; set; }

    #endregion
  }
}
