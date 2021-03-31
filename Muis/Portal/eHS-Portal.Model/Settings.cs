using System;
using System.Collections.Generic;
using System.Text;

namespace eHS.Portal.Model
{
  public enum SettingsType
  {
    WorkingDaysNormal = 100,
    WorkingDaysExpress = 101,
    RFANormal = 102,
    RFAExpress = 103,

    RenewalHawker = 104,
    RenewalCanteen = 105,
    RenewalRestaurant = 106,
    RenewalHalalSection = 107,
    RenewalFoodKiosk = 108,
    RenewalFoodStation = 109,
    RenewalSnakBarBakery = 110,
    RenewalShotTerm = 111,

    RenewalCentralKitchen = 112,
    RenewalCatering = 113,
    RenewalPreSchoolKitchen = 114,

    RenewalTriggerProduct = 115,
    RenewalTriggerWholePlant = 116,
    RenewalTriggerPolutry = 117,
    RenewalTriggerEndorsement = 118,
    RenewalTriggerStorageFacility = 119,

    CertificateRenewalTriggerGSO = 120,

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
