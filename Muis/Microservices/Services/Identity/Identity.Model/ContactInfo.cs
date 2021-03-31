using System;

namespace Identity.Model
{
  public enum ContactInfoType
  {
    Office,
    OfficeExt,
    Home,
    Fax,
    Mobile,
    Email,
    AltEmail,
    Others = 99
  }

  public class ContactInfo
  {
    public long ID { get; set; }

    public ContactInfoType Type { get; set; }

    public string Value { get; set; }

    public bool IsPrimary { get; set; }

    public Guid PersonID { get; set; }

    public DateTimeOffset CreatedOn { get; set; }

    public DateTimeOffset ModifiedOn { get; set; }

    public bool IsDeleted { get; set; }

    #region Generated properties

    public string TypeText { get; set; }

    #endregion
  }
}
