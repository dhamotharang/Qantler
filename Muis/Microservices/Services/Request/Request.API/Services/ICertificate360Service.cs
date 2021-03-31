using Core.Model;
using Request.API.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Request.API.Services
{
  public interface ICertificate360Service
  {
    Task<IList<Model.Certificate360>> GetCertificatesForRenewal
      (Scheme scheme, SubScheme? subScheme);

    /// <summary>
    /// Get Certificate360 by Number
    /// </summary>
    /// <param name="CertificateNo"></param>
    /// <returns></returns>
    Task<Model.Certificate360> GetCertificateByCertNo(string CertificateNo);

    /// <summary>
    /// Get Certificate360 by ID
    /// </summary>
    /// <param name="ID"></param>
    /// <returns></returns>
    Task<Model.Certificate360> GetCertificateByCertID(long ID);

    /// <summary>
    /// Get Certificate360 History by certificate id
    /// </summary>
    /// <param name="ID"></param>
    /// <returns></returns>
    Task<IList<Model.Certificate360History>> GetCertHistory(long ID);

    /// <summary>
    /// Get Certificate360 Menus by certificate id
    /// </summary>
    /// <param name="ID"></param>
    /// <returns></returns>
    Task<IList<Model.Menu>> GetCertMenus(long ID);

    /// <summary>
    /// Get Certificate360 Ingredients by certificate id
    /// </summary>
    /// <param name="ID"></param>
    /// <returns></returns>
    Task<IList<Model.Ingredient>> GetCertIngredients(long ID);

    /// <summary>
    /// Get certificate360 With ingredient
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    Task<IList<Model.Certificate360>> GetCertWithIngredient(Certificate360IngredientFilter filter);

    Task<long> InsertAutoRenewalTriggerLog(string Number, DateTimeOffset? ExpiresOn);

    Task<IList<Model.Certificate360>> GetCertificates(Certificate360Filter filter);
  }
}
