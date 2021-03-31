using System;
using System.Collections.Generic;
using System.Text;

namespace eHS.Portal.Model
{
  public class BasePremise
  {
    public long ID { get; set; }

    public PremiseType Type { get; set; }

    public bool IsLocal { get; set; }

    public string Name { get; set; }

    public string Area { get; set; }

    public string Schedule { get; set; }

    public string BlockNo { get; set; }

    public string UnitNo { get; set; }

    public string FloorNo { get; set; }

    public string BuildingName { get; set; }

    public string Address1 { get; set; }

    public string Address2 { get; set; }

    public string City { get; set; }

    public string Province { get; set; }

    public string Country { get; set; }

    public string Postal { get; set; }

    public double Longitude { get; set; }

    public double Latitude { get; set; }

    public bool IsPrimary { get; set; }

    public DateTimeOffset? CreatedOn { get; set; }

    public DateTimeOffset? ModifiedOn { get; set; }

    public bool IsDeleted { get; set; }

    #region Generated properties

    public string TypeText { get; set; }

    #endregion
  }
}
