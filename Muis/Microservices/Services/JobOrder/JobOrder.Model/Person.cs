using System;
using System.Collections.Generic;
using Core.Model;

namespace JobOrder.Model
{
  public class Person
  {
    public Guid ID { get; set; }

    public string Salutation { get; set; }

    public string Name { get; set; }

    public string Nationality { get; set; }

    public string Gender { get; set; }

    public DateTimeOffset? DOB { get; set; }

    public string Designation { get; set; }

    public string DesignationOther { get; set; }

    public IDType IDType { get; set; }

    public string AltID { get; set; }

    public string PassportIssuingCountry { get; set; }

    public DateTimeOffset CreatedOn { get; set; }

    public DateTimeOffset ModifiedOn { get; set; }

    public bool IsDeleted { get; set; }

    #region Generated properties

    public string IDTypeText { get; set; }

    public IList<ContactInfo> ContactInfos { get; set; }

    #endregion
  }
}
