using System;

namespace eHS.Portal.Model
{
  public enum RiskCategory
  {
    NotCategorized = 999,
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

    public string Text { get; set; }

    public string SubText { get; set; }

    public RiskCategory RiskCategory { get; set; }

    public IngredientStatus? Status { get; set; }

    public string SupplierName { get; set; }

    public string BrandName { get; set; }

    public CertifyingBodyStatus? CertifyingBodyStatus { get; set; }

    public string CertifyingBodyName { get; set; }

    public bool? Approved { get; set; }

    public string Remarks { get; set; }

    public Guid? ReviewedBy { get; set; }

    public string ReviewedByName { get; set; }

    public DateTimeOffset? ReviewedOn { get; set; }

    public ChangeType ChangeType { get; set; }

    public long RequestID { get; set; }

    public DateTimeOffset? CreatedOn { get; set; }

    public DateTimeOffset? ModifiedOn { get; set; }

    public bool IsDeleted { get; set; }

    public bool Undeclared { get; set; }

    #region Generated Properties

    public string RiskCategoryText { get; set; }

    public string IngredientStatusText { get; set; }

    public string CertifyingBodyStatusText { get; set; }

    #endregion
  }
}
