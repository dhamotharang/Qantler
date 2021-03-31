using eHS.Portal.Model;
using System;

namespace eHS.Portal.Client
{
  public class HalalLibraryOptions
  {
    public string Name { get; set; }

    public string Brand { get; set; }

    public string Supplier { get; set; }

    public string CertifyingBody { get; set; }

    public RiskCategory? RiskCategory { get; set; }

    public HLIngredientStatus? Status { get; set; }

    public string VerifiedBy { get; set; }
  }
}
