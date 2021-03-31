using System;
using System.Collections.Generic;
using System.Text;
using Core.Model;
using Request.Model;

namespace Request.API.Helpers
{
  public class CertificateHelper
  {
    static readonly IDictionary<string, string> SchemePrefix = new Dictionary<string, string>()
    {
      { $"{Scheme.FoodManufacturing}:{SubScheme.Product}", "FMPR" },
      { $"{Scheme.FoodManufacturing}:{SubScheme.WholePlant}", "FMWP" },
      { $"{Scheme.Poultry}", "PABT" },
      { $"{Scheme.Endorsement}", "ENOR" },
      { $"{Scheme.StorageFacility}", "SFAC" },
      { $"{Scheme.FoodPreparationArea}:{SubScheme.CentralKitchen}", "FPCK" },
      { $"{Scheme.FoodPreparationArea}:{SubScheme.Catering}", "FPCA" },
      { $"{Scheme.FoodPreparationArea}:{SubScheme.PreSchoolKitchen}", "FPSK" },
      { $"{Scheme.EatingEstablishment}:{SubScheme.Canteen}", "EESC" },
      { $"{Scheme.EatingEstablishment}:{SubScheme.HalalSection}", "EEHS" },
      { $"{Scheme.EatingEstablishment}:{SubScheme.Hawker}", "EEH" },
      { $"{Scheme.EatingEstablishment}:{SubScheme.FoodKiosk}", "EEFK" },
      { $"{Scheme.EatingEstablishment}:{SubScheme.FoodStation}", "EEFS" },
      { $"{Scheme.EatingEstablishment}:{SubScheme.Restaurant}", "EERT" },
      { $"{Scheme.EatingEstablishment}:{SubScheme.ShortTerm}", "EEST" },
      { $"{Scheme.EatingEstablishment}:{SubScheme.SnackBarBakery}", "EESB" },
      { $"{Scheme.EatingEstablishment}:{SubScheme.StaffCanteen}", "EESC" },
    };

    public static string Prefix (Scheme scheme, SubScheme? subScheme)
    {
      return SchemePrefix[FormatKey(scheme, subScheme)];
    }

    public static CertificateTemplate CertificateTemplate(Scheme scheme, SubScheme? subScheme)
    {
      switch(scheme)
      {
        case Scheme.FoodManufacturing:
          if (subScheme == SubScheme.Product)
          {
            return Model.CertificateTemplate.Product;
          }
          return Model.CertificateTemplate.WholePlant;
        case Scheme.StorageFacility:
          return Model.CertificateTemplate.Storage;
        case Scheme.Poultry:
          return Model.CertificateTemplate.Poultry;
        case Scheme.Endorsement:
          return Model.CertificateTemplate.Endorsement;
        case Scheme.FoodPreparationArea:
          if (subScheme == SubScheme.Catering)
          {
            return Model.CertificateTemplate.Catering;
          }

          return Model.CertificateTemplate.FoodPreparationArea;
      }
      return Model.CertificateTemplate.EatingEstablishment;
    }

    public static string GenerateCertificateNo (Scheme scheme, SubScheme? subScheme,
      int sequenceNo)
    {
      var prefix = $"{SchemePrefix[FormatKey(scheme, subScheme)]}{DateTime.UtcNow:yyyy}";
      var sufix = $"{sequenceNo}";

      var sb = new StringBuilder();
      sb.Append(prefix);
      sb.Append('0', 7 - sufix.Length);
      sb.Append(sufix);

      return sb.ToString();
    }

    public static string GenerateSerialNo(int sequenceNo)
    {
      var prefix = $"C{DateTime.UtcNow:yyyy}";
      var sufix = $"{sequenceNo}";

      var sb = new StringBuilder();
      sb.Append(prefix);
      sb.Append('0', 10 - sufix.Length);
      sb.Append(sufix);

      return sb.ToString();
    }

    public static CertificateBatch CertificateBatch(DateTimeOffset refDate, Scheme scheme,
      SubScheme? subScheme)
    {
      var key = FormatKey(scheme, subScheme);
      var prefix = SchemePrefix[key];
      var template = CertificateTemplate(scheme, subScheme);

      string color;
      switch (template)
      {
        case Model.CertificateTemplate.Product:
        case Model.CertificateTemplate.WholePlant:
          color = "Blue";
          break;
        case Model.CertificateTemplate.Poultry:
        case Model.CertificateTemplate.Storage:
          color = "Brown";
          break;
        case Model.CertificateTemplate.FoodPreparationArea:
          color = "Orange";
          prefix = "FPA";
          break;
        case Model.CertificateTemplate.Catering:
          color = "Green";
          break;
        default:
          color = "Green";
          prefix = "EE";
          break;
      }

      return new CertificateBatch
      {
        Code = $"{prefix}{refDate:yyyyMMdd}",
        Description = $"{color} Certificate - {template}",
        Template = template,
        Status = CertificateBatchStatus.Pending
      };
    }

    public static (DateTimeOffset, DateTimeOffset) CalculateCertificateDuration(
      RequestType type,
      DateTimeOffset? currentExpiry,
      DateTimeOffset? accountExpiry,
      int durationInYears)
    {
      if (type == RequestType.HC03 || type == RequestType.HC02)
      {
        return (DateTimeOffset.UtcNow.Date, currentExpiry.Value);
      }

      var now = DateTimeOffset.UtcNow.Date;

      if (   type == RequestType.New
          && (accountExpiry?.Date ?? now) > now)
      {
        return (Trunctate(now.AddMonths(1)), accountExpiry.Value);
      }

      var from = now;
      if (   type == RequestType.Renewal
          && (currentExpiry?.Date ?? now) > now)
      {
        from = currentExpiry.Value.Date;
      }

      from = Trunctate(from.AddMonths(1));
      return (from, from.AddYears(durationInYears).AddDays(-1));
    }

    static string FormatKey (Scheme scheme, SubScheme? subScheme)
    {
      return subScheme == null ? $"{scheme}" : $"{scheme}:{subScheme}";
    }

    static DateTime Trunctate(DateTime date)
    {
      return new DateTime(date.Year, date.Month, 1);
    }
  }
}