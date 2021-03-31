using System;
using System.Collections.Generic;

namespace eHS.Portal.Model
{
  public class RequestLineItem
  {
    public long ID { get; set; }

    public Scheme? Scheme { get; set; }

    public SubScheme? SubScheme { get; set; }

    public long? ChecklistHistoryID { get; set; }

    public long RequestID { get; set; }

    public long? CertificateID { get; set; }

    public DateTimeOffset? CreatedOn { get; set; }

    public DateTimeOffset? ModifiedOn { get; set; }

    public bool? IsDeleted { get; set; }

    #region Generated properties

    public string SchemeText { get; set; }

    public string SubSchemeText { get; set; }

    public IList<Characteristic> Characteristics { get; set; }

    #endregion
  }
}
