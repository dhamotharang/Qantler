using System;

namespace Core.Model
{
  public enum Scheme
  {
    EatingEstablishment = 100,
    FoodPreparationArea = 200,
    FoodManufacturing = 300,
    Poultry = 400,
    Endorsement = 500,
    StorageFacility = 600
  }

  public enum SubScheme
  {
    Canteen = 101,
    HalalSection = 102,
    Hawker = 103,
    FoodKiosk = 104,
    FoodStation = 105,
    Restaurant = 106,
    ShortTerm = 107,
    SnackBarBakery = 108,
    StaffCanteen = 109,
    CentralKitchen = 201,
    Catering = 202,
    PreSchoolKitchen = 203,
    Product = 301,
    WholePlant = 302
  }

  public static class ProductClassification
  {
    public const string OwnProduct = "Own Product";
    public const string BranchOwner = "Brand Owner";
    public const string Manufacturer = "Manufacturer";
  }
}
