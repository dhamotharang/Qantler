using System;

namespace Request.Model
{
  public enum RequestCharType
  {
    Duration,
    OtherInfo,
    IssuedCertificate,
    CertificateExpiry,
    NoOfCopies,
    NoOfProducts,
    ProductClassification,
    EndorsementLabelCount,
    StartDate,
    EndDate,
    PoultryTags,
    CertificateIssuedDate,
    OldCertificateIssuedDate,
    OldCertificateExpiry,
    SlaughterDate
  }

  public class Characteristic
  {
    public long ID { get; set; }

    public RequestCharType Type { get; set; }

    public string Value { get; set; }    

    public DateTimeOffset? CreatedOn { get; set; }

    public DateTimeOffset? ModifiedOn { get; set; }

    public bool IsDeleted { get; set; }

    #region Generated properties

    public string TypeText { get; set; }

    #endregion
  }
}
