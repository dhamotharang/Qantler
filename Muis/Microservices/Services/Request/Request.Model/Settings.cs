using System;
using Core.Model;

namespace Request.Model
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

    CertificateRenewalTriggerGSO = 120
  }

  public class Settings : BaseSettings
  {
    public SettingsType Type { get; set; }
  }
}
