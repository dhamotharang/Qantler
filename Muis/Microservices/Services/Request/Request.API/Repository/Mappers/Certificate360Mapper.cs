using Core.Model;
using Request.API.Models;
using Request.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Request.API.Repository.Mappers
{
  public class Certificate360Mapper
  {

    readonly Dictionary<long, Certificate360> _certificateCache =
     new Dictionary<long, Certificate360>();
    readonly Dictionary<long, Certificate360History> _certHistoryCache =
     new Dictionary<long, Certificate360History>();
    readonly Dictionary<long, Menu> _certMenuCache =
    new Dictionary<long, Menu>();
    readonly Dictionary<long, Ingredient> _certIngCache =
   new Dictionary<long, Ingredient>();
    readonly Dictionary<long, Premise> _certPremiseCache =
  new Dictionary<long, Premise>();
    readonly Dictionary<Guid, Customer> _certCustomerCache =
  new Dictionary<Guid, Customer>();
    readonly Dictionary<long, HalalTeam> _certHalalTeamCache =
  new Dictionary<long, HalalTeam>();

    public Certificate360 Map(Certificate360 certificate)
    {
      return certificate;
    }

    public Certificate360 Map(Certificate360 certificate360,
      Premise premise = null,
      Menu menu = null)
    {
      if (!_certificateCache.TryGetValue(certificate360.ID, out Certificate360 result))
      {
        result = certificate360;
        certificate360.Premise = premise;
        _certificateCache[certificate360.ID] = certificate360;
      }

      if ((menu?.ID ?? 0) > 0
          && !_certMenuCache.ContainsKey(menu.ID))
      {
        _certMenuCache[menu.ID] = menu;
        if (result.Menus == null)
        {
          result.Menus = new List<Menu>();
        }
        result.Menus.Add(menu);
      }

      return result;
    }

    public Certificate360 Map(Certificate360 certificate360,
      Customer customer,
      Code customerCode,
      Code customerGroupCode,
      Officer cutomerOfficer,
      Premise premise)
    {
      if (!_certificateCache.TryGetValue(certificate360.ID, out Certificate360 result))
      {
        result = certificate360;
        certificate360.Premise = premise;
        certificate360.Customer = customer;

        if (customer != null
         && (customerCode?.ID ?? 0L) != 0L)
        {
          if (certificate360.Customer.Code == null)
          {
            certificate360.Customer.Code = new Code();
          }

          certificate360.Customer.Code = customerCode;
        }

        if (customer != null
        && (customerGroupCode?.ID ?? 0L) != 0L)
        {
          if (certificate360.Customer.GroupCode == null)
          {
            certificate360.Customer.GroupCode = new Code();
          }

          certificate360.Customer.GroupCode = customerGroupCode;
        }

        _certificateCache[certificate360.ID] = certificate360;

        if (customer != null
        && (cutomerOfficer?.ID ?? Guid.Empty) != Guid.Empty)
        {
          if (certificate360.Customer.Officer == null)
          {
            certificate360.Customer.Officer = new Officer();
          }

          certificate360.Customer.Officer = cutomerOfficer;
        }

        _certificateCache[certificate360.ID] = certificate360;
      }

      return result;
    }

    public Certificate360 Map(Certificate360 certificate,
     Premise certPremise,
     HalalTeam certHalalTeam)
    {
      if (!_certificateCache.TryGetValue(certificate.ID, out Certificate360 result))
      {
        certificate.Teams = new List<HalalTeam>();
        certificate.Premise = new Premise();

        _certificateCache[certificate.ID] = certificate;
        result = certificate;
      }

      if (certPremise.ID != 0)
      {
        if (!_certPremiseCache.TryGetValue(certPremise.ID, out Premise certPremiseOut))
        {
          _certPremiseCache[certPremise.ID] = certPremise;
          certPremiseOut = certPremise;
          result.Premise = certPremise;
        }
      }

      if (certHalalTeam.ID != 0)
      {
        if (!_certHalalTeamCache.ContainsKey(certHalalTeam.ID))
        {
          _certHalalTeamCache[certHalalTeam.ID] = certHalalTeam;
          result.Teams.Add(certHalalTeam);
        }
      }

      return result;
    }

    public Certificate360 Map(Certificate360 certificate, Certificate360History certHistory,
     Menu certMenu, Ingredient certIng,
     Premise certPremise, Customer certCustomer, HalalTeam certHalalTeam)
    {
      if (!_certificateCache.TryGetValue(certificate.ID, out Certificate360 result))
      {
        certificate.Teams = new List<HalalTeam>();
        //certificate.History = new List<CertificateHistory>();
        certificate.Menus = new List<Menu>();
        //certificate.Ingredients = new List<Ingredient>();
        certificate.Premise = new Premise();
        //certificate.Customer = new Customer();

        _certificateCache[certificate.ID] = certificate;
        result = certificate;
      }
      if (certHistory.ID != 0)
      {
        if (!_certHistoryCache.TryGetValue(certHistory.ID, out Certificate360History certHistOut))
        {
          _certHistoryCache[certHistory.ID] = certHistory;
          certHistOut = certHistory;
          //result.History.Add(certHistory);
        }
      }

      if (certMenu.ID != 0)
      {
        if (!_certMenuCache.TryGetValue(certMenu.ID, out Menu certMenuOut))
        {
          _certMenuCache[certMenu.ID] = certMenu;
          certMenuOut = certMenu;
          result.Menus.Add(certMenu);
        }
      }

      if (certIng.ID != 0)
      {
        if (!_certIngCache.TryGetValue(certIng.ID, out Ingredient certIngOut))
        {
          _certIngCache[certIng.ID] = certIng;
          certIngOut = certIng;
          //result.Ingredients.Add(certIng);
        }
      }

      if (certPremise.ID != 0)
      {
        if (!_certPremiseCache.TryGetValue(certPremise.ID, out Premise certPremiseOut))
        {
          _certPremiseCache[certPremise.ID] = certPremise;
          certPremiseOut = certPremise;
          result.Premise = certPremise;
        }
      }


      if (!_certCustomerCache.TryGetValue(certCustomer.ID, out Customer certCustomerOut))
      {
        _certCustomerCache[certCustomer.ID] = certCustomer;
        certCustomerOut = certCustomer;
        //result.Customer = certCustomer;
      }

      if (certHalalTeam.ID != 0)
      {
        if (!_certHalalTeamCache.ContainsKey(certHalalTeam.ID))
        {
          _certHalalTeamCache[certHalalTeam.ID] = certHalalTeam;
          result.Teams.Add(certHalalTeam);
        }
      }

      return result;
    }
  }
}
