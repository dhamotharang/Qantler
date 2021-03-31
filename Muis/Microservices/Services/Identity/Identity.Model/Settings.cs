using System;
using Core.Model;

namespace Identity.Model
{
  public enum SettingsType
  {
    CertificateRenewalTriggerEatingEstablishmentHawker = 101,
    CertificateRenewalTriggerEatingEstablishmentCanteen = 102,
    CertificateRenewalTriggerEatingEstablishmentRestaurant = 103,
    CertificateRenewalTriggerEatingEstablishmentHalalSection = 104,
    CertificateRenewalTriggerEatingEstablishmentFoodKiosk = 105,
    CertificateRenewalTriggerEatingEstablishmentFoodStation = 106,
    CertificateRenewalTriggerEatingEstablishmentSnakBarBakery = 107,
    CertificateRenewalTriggerEatingEstablishmentShotTerm = 108,

    CertificateRenewalTriggerFoodPreperationAreaCentralKitchen = 201,
    CertificateRenewalTriggerFoodPreperationAreaCatering = 202,
    CertificateRenewalTriggerFoodPreperationAreaPreSchoolKitchen = 203,

    CertificateRenewalTriggerProduct = 301,
    CertificateRenewalTriggerWholePlant = 302,
    CertificateRenewalTriggerPolutry = 400,
    CertificateRenewalTriggerEndorsement = 500,
    CertificateRenewalTriggerStorageFacility = 600,

    CertificateRenewalTriggerGSO = 999
  }

  public class Settings : BaseSettings
  {
    public SettingsType Type { get; set; }
  }
}
