using System;
using System.Collections.Generic;
using Request.Model;

namespace Request.API.Repository.Mappers
{
  public class CertificateMapper
  {
    readonly IDictionary<long, Certificate> _cache = new Dictionary<long, Certificate>();

    readonly IDictionary<long, Menu> _menuCache = new Dictionary<long, Menu>();

    public Certificate Map(Certificate certificate,
      Premise premise = null,
      Premise mailingPremise = null,
      Menu menu = null)
    {
      if (!_cache.TryGetValue(certificate.ID, out Certificate result))
      {
        result = certificate;
        certificate.Premise = premise;
        certificate.MailingPremise = mailingPremise;

        _cache[certificate.ID] = certificate;
      }

      if ((menu?.ID ?? 0) > 0
          && !_menuCache.ContainsKey(menu.ID))
      {
        _menuCache[menu.ID] = menu;

        if (result.Menus == null)
        {
          result.Menus = new List<Menu>();
        }
        result.Menus.Add(menu);
      }

      return result;
    }

    public Certificate Map(Certificate certificate,
     Premise premise = null,
     Premise mailingPremise = null,
     Code code = null)
    {
      if (!_cache.TryGetValue(certificate.ID, out Certificate result))
      {
        result = certificate;
        certificate.Premise = premise;
        certificate.MailingPremise = mailingPremise;
        certificate.CustomerCode = code;
        _cache[certificate.ID] = certificate;
      }
      return certificate;
    }
  }
}
