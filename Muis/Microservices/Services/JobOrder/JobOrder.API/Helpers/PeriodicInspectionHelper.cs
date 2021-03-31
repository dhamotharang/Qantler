using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Model;
using JobOrder.API.Repository;
using JobOrder.API.Services;
using JobOrder.Model;

namespace JobOrder.API.Helpers
{
  public static class PeriodicInspectionHelper
  {
    const int TotalNoOfDays = 365;

    static IDictionary<int, SettingsType> _schemeSettingsTypeMap =
      new Dictionary<int, SettingsType>
    {
        { 101, SettingsType.PeriodicCanteen },
        { 102, SettingsType.PeriodicHalalSection },
        { 103, SettingsType.PeriodicHawker },
        { 104, SettingsType.PeriodicFoodKiosk },
        { 105, SettingsType.PeriodicFoodStation },
        { 106, SettingsType.PeriodicRestaurant },
        { 107, SettingsType.PeriodicShotTerm },
        { 108, SettingsType.PeriodicSnakBarBakery },
        { 201, SettingsType.PeriodicCentralKitchen },
        { 202, SettingsType.PeriodicCatering },
        { 203, SettingsType.PeriodicPreSchoolKitchen },
        { 301, SettingsType.PeriodicProduct },
        { 302, SettingsType.PeriodicWholePlant },
        { 400, SettingsType.PeriodicPolutry },
        { 500, SettingsType.PeriodicEndorsement },
        { 600, SettingsType.PeriodicStorageFacility }
    };

    public async static Task<DateTimeOffset?> CalculateNextTargetScheduledDate(
      DateTimeOffset? LastScheduled, List<Certificate> certificates, int? grade,
      bool? isHighPriority, ISettingsService settingsService)
    {
      var interval = await CalculateInterval(certificates, grade, isHighPriority, settingsService);

      var expiresOn = certificates.Min(e => e.ExpiresOn);

      var blockPeriod = CalculateBlockPeriod(certificates, interval, expiresOn, settingsService);

      var nextTargetInspection = LastScheduled.Value.AddDays(interval);

      DateTimeOffset? isRet = null;
      if (!(nextTargetInspection > blockPeriod))
        isRet = nextTargetInspection;

      return isRet;
    }
    public async static Task<int> CalculateInterval(IList<Certificate> certificates, int? grade,
      bool? isHighPriority, ISettingsService settingsService)
    {
      var settings = await settingsService.GetJobOrderSettings();

      var gradeFactor = 1;
      if (grade != null)
      {
        gradeFactor = Convert.ToInt32(settings.Where(e => Convert.ToInt32(e.Type) == grade)
          .ToList()[0].Value);
      }

      var highPriorityFactor = 1;
      if (isHighPriority.Value)
      {
        highPriorityFactor = Convert.ToInt32(settings.Where(e => Convert.ToInt32(e.Type)
        == (int)SettingsType.PeriodicHighPriority).ToList()[0].Value);
      }

      var vertificateFactor = 0;

      foreach (Certificate certificate in certificates)
      {
        var settingsType = GetSchemeSettingsType(certificate.Scheme, certificate.SubScheme);
        vertificateFactor += Convert.ToInt32(settings.Where(e => Convert.ToInt32(e.Type)
        == settingsType).ToList()[0].Value);
      }

      vertificateFactor = vertificateFactor / certificates.Count();

      return 365 / (1 + (vertificateFactor * gradeFactor * highPriorityFactor));
    }

    static DateTimeOffset CalculateBlockPeriod(List<Certificate> certificates, int interval,
      DateTimeOffset? expiresOn, ISettingsService settingsService)
    {
      return expiresOn.Value.AddDays(-interval);
    }

    static int GetSchemeSettingsType(Scheme scheme, SubScheme? subScheme)
    {
      int key = subScheme != null ? (int)subScheme : (int)scheme;
      return (int)_schemeSettingsTypeMap[key];
    }
  }
}
