using System;
using Core.Model;

namespace HalalLibrary.Model
{
  public enum RiskCategory
  {
    Uncategorized = 999,
    NonHalal = 500,
    HighRisk = 400,
    MediumHighRisk = 300,
    MediumLowRisk = 200,
    LowRisk = 100
  }

  public enum IngredientStatus
  {
    Unverified,
    Verified
  }

  public class Ingredient
  {
    public long ID { get; set; }

    public string Name { get; set; }

    public string Brand { get; set; }

    public RiskCategory RiskCategory { get; set; }

    public IngredientStatus Status { get; set; }

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
