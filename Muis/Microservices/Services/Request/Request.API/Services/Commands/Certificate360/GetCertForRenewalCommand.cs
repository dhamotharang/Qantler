using Core.API;
using Core.API.Repository;
using Core.EventBus;
using Request.API.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Model;

namespace Request.API.Services.Commands.Certificate360
{
  public class GetCertForRenewalCommand : IUnitOfWorkCommand<IList<Model.Certificate360>>
  {
    readonly Scheme _scheme;
    readonly SubScheme? _subScheme;
    public GetCertForRenewalCommand(Scheme scheme, SubScheme? subScheme)
    {
      _scheme = scheme;
      _subScheme = subScheme;
    }

    public async Task<IList<Model.Certificate360>> Invoke(IUnitOfWork unitOfWork)
    {
      IList<Model.Certificate360> certs = null;
      if (_scheme == Scheme.EatingEstablishment)
      {
        if (_subScheme != null)
        {
          if (_subScheme == SubScheme.Canteen)
          {
            var sett = await DbContext.From(unitOfWork).Settings.GetSettingsByType
              (Model.SettingsType.RenewalCanteen);
            certs = await GetCerts(unitOfWork, certs, sett, _scheme, _subScheme);
          }
          else if (_subScheme == SubScheme.HalalSection)
          {
            var sett = await DbContext.From(unitOfWork).Settings.GetSettingsByType
              (Model.SettingsType.RenewalHalalSection);
            certs = await GetCerts(unitOfWork, certs, sett, _scheme, _subScheme);
          }
          else if (_subScheme == SubScheme.Hawker)
          {
            var sett = await DbContext.From(unitOfWork).Settings.GetSettingsByType
              (Model.SettingsType.RenewalHawker);
            certs = await GetCerts(unitOfWork, certs, sett, _scheme, _subScheme);
          }
          else if (_subScheme == SubScheme.FoodKiosk)
          {
            var sett = await DbContext.From(unitOfWork).Settings.GetSettingsByType
              (Model.SettingsType.RenewalFoodKiosk);
            certs = await GetCerts(unitOfWork, certs, sett, _scheme, _subScheme);
          }
          else if (_subScheme == SubScheme.FoodStation)
          {
            var sett = await DbContext.From(unitOfWork).Settings.GetSettingsByType
              (Model.SettingsType.RenewalFoodStation);
            certs = await GetCerts(unitOfWork, certs, sett, _scheme, _subScheme);
          }
          else if (_subScheme == SubScheme.Restaurant)
          {
            var sett = await DbContext.From(unitOfWork).Settings.GetSettingsByType
              (Model.SettingsType.RenewalRestaurant);
            certs = await GetCerts(unitOfWork, certs, sett, _scheme, _subScheme);
          }
          else if (_subScheme == SubScheme.SnackBarBakery)
          {
            var sett = await DbContext.From(unitOfWork).Settings.GetSettingsByType
              (Model.SettingsType.RenewalSnakBarBakery);
            certs = await GetCerts(unitOfWork, certs, sett, _scheme, _subScheme);
          }
          else if (_subScheme == SubScheme.StaffCanteen)
          {
            //var sett = await DbContext.From(unitOfWork).Settings.GetSettingsByType
            //  (Model.SettingsType.staf);
            //certs = await GetCerts(unitOfWork, certs, sett);
          }
        }
      }
      else if (_scheme == Scheme.FoodPreparationArea)
      {
        if (_subScheme != null)
        {
          if (_subScheme == SubScheme.CentralKitchen)
          {
            var sett = await DbContext.From(unitOfWork).Settings.GetSettingsByType
              (Model.SettingsType.RenewalCentralKitchen);
            certs = await GetCerts(unitOfWork, certs, sett, _scheme, _subScheme);
          }
          else if (_subScheme == SubScheme.Catering)
          {
            var sett = await DbContext.From(unitOfWork).Settings.GetSettingsByType
              (Model.SettingsType.RenewalCatering);
            certs = await GetCerts(unitOfWork, certs, sett, _scheme, _subScheme);
          }
          else if (_subScheme == SubScheme.PreSchoolKitchen)
          {
            var sett = await DbContext.From(unitOfWork).Settings.GetSettingsByType
              (Model.SettingsType.RenewalPreSchoolKitchen);
            certs = await GetCerts(unitOfWork, certs, sett, _scheme, _subScheme);
          }
        }
      }
      else if (_scheme == Scheme.FoodManufacturing)
      {
        if (_subScheme != null)
        {
          if (_subScheme == SubScheme.Product)
          {
            var sett = await DbContext.From(unitOfWork).Settings.GetSettingsByType
              (Model.SettingsType.RenewalTriggerProduct);
            certs = await GetCerts(unitOfWork, certs, sett, _scheme, _subScheme);
          }
          else if (_subScheme == SubScheme.WholePlant)
          {
            var sett = await DbContext.From(unitOfWork).Settings.GetSettingsByType
              (Model.SettingsType.RenewalTriggerWholePlant);
            certs = await GetCerts(unitOfWork, certs, sett, _scheme, _subScheme);
          }
        }
      }
      else if (_scheme == Scheme.Poultry)
      {
        var sett = await DbContext.From(unitOfWork).Settings.GetSettingsByType
              (Model.SettingsType.RenewalTriggerPolutry);
        certs = await GetCerts(unitOfWork, certs, sett, _scheme, _subScheme);
      }
      else if (_scheme == Scheme.Endorsement)
      {
        var sett = await DbContext.From(unitOfWork).Settings.GetSettingsByType
              (Model.SettingsType.RenewalTriggerEndorsement);
        certs = await GetCerts(unitOfWork, certs, sett, _scheme, _subScheme);
      }
      else if (_scheme == Scheme.StorageFacility)
      {
        var sett = await DbContext.From(unitOfWork).Settings.GetSettingsByType
              (Model.SettingsType.RenewalTriggerStorageFacility);
        certs = await GetCerts(unitOfWork, certs, sett, _scheme, _subScheme);
      }
      return certs;
    }

    private static async Task<IList<Model.Certificate360>> GetCerts(IUnitOfWork unitOfWork, IList<Model.Certificate360> certs, Model.Settings sett, Scheme scheme, SubScheme? subScheme)
    {
      if (sett != null && !string.IsNullOrEmpty(sett.Value))
      {
        certs = await DbContext.From(unitOfWork).Certificate360.GetCertificatesForRenewal
(Convert.ToInt32(sett.Value), scheme, subScheme);
      }

      return certs;
    }
  }
}
