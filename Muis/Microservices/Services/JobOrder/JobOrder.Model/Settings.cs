using System;
using Core.Model;

namespace JobOrder.Model
{
  public enum SettingsType
  {
    PeriodicHawker = 200,
    PeriodicCanteen = 201,
    PeriodicRestaurant = 202,
    PeriodicHalalSection = 203,
    PeriodicFoodKiosk = 204,
    PeriodicFoodStation = 205,
    PeriodicSnakBarBakery = 206,
    PeriodicShotTerm = 207,

    PeriodicCentralKitchen = 208,
    PeriodicCatering = 209,
    PeriodicPreSchoolKitchen = 210,

    PeriodicProduct = 211,
    PeriodicWholePlant = 212,
    PeriodicPolutry = 213,
    PeriodicEndorsement = 214,
    PeriodicStorageFacility = 215,

    PeriodicHighPriority = 216,
    PeriodicGradingA = 217,
    PeriodicGradingB = 218,
    PeriodicGradingC = 219
  }

  public class Settings : BaseSettings
  {
    public SettingsType Type { get; set; }
  }
}
