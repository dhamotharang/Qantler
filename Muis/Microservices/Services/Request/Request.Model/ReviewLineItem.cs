using System;
using Core.Model;

namespace Request.Model
{
  public class ReviewLineItem
  {
    public long ID { get; set; }

    public Scheme? Scheme { get; set; }

    public SubScheme? SubScheme { get; set; }

    public bool? Approved { get; set; }

    public string Remarks { get; set; }

    public long ReviewID { get; set; }

    #region Generated properties

    public string SchemeText { get; set; }

    public string SubSchemeText { get; set; }

    #endregion
  }
}
