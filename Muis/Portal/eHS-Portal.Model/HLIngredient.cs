using System;
namespace eHS.Portal.Model
{
  public enum HLIngredientStatus
  { 
    Unverified,
    Verified
  }

  public class HLIngredient
  {
    public long ID { get; set; }

    public string Name { get; set; }

    public string Brand { get; set; }

    public RiskCategory RiskCategory { get; set; }

    public HLIngredientStatus Status { get; set; }

    public long? SupplierID { get; set; }

    public long? CertifyingBodyID { get; set; }

    public Guid? VerifiedByID { get; set; }

    public DateTimeOffset? CreatedOn { get; set; }

    public DateTimeOffset? ModifiedOn { get; set; }

    public bool IsDeleted { get; set; }

    #region Generated properties

    public string RiskCategoryText { get; set; }

    public string StatusText { get; set; }

    public Supplier Supplier { get; set; }

    public CertifyingBody CertifyingBody { get; set; }

    public Officer VerifiedBy { get; set; }

    #endregion
  }
}
