using Request.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Model;

namespace Request.API.Repository
{
  public interface ICertificate360Repository
  {
    Task<IList<Certificate360>> GetCertificatesForRenewal
      (int RenewalPeriod, Scheme scheme, SubScheme? subScheme);

    /// <summary>
    /// Query certificate360 by specified filter.
    /// </summary>
    /// <param name="filter">the filter options</param>
    Task<IList<Certificate360>> Certificate360Filter(Certificate360Filter filter);

    /// <summary>
    /// Get Certificate360 by number
    /// </summary>
    /// <param name="CertificateNo"></param>
    /// <returns></returns>
    Task<Certificate360> GetCertificateByCertNo(string CertificateNo);

    /// <summary>
    /// Get Certificate360 by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<Certificate360> GetCertificateByCertID(long ID);

    /// <summary>
    /// Get Certificate360 History by certificate id
    /// </summary>
    /// <param name="certificateID"></param>
    /// <returns></returns>
    Task<IList<Certificate360History>> GetCertificate360History
      (long certificateID);

    /// <summary>
    /// Get Certificate360 Ingredients by certificate id
    /// </summary>
    /// <param name="certificateID"></param>
    /// <returns></returns>
    Task<IList<Ingredient>> GetCertificate360Ingredients
     (long certificateID);

    /// <summary>
    /// Get Certificate360 with Ingredient
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    Task<IList<Certificate360>> GetCertificate360WithIngredient
      (Certificate360IngredientFilter filter);

    /// <summary>
    /// Get Certificate360 Menu by certificate id
    /// </summary>
    /// <param name="certificateID"></param>
    /// <returns></returns>
    Task<IList<Menu>> GetCertificate360Menus
     (long certificateID);

    /// <summary>
    /// Insert Certificate360 instance.
    /// </summary>
    Task<long> InsertCertificate360(Certificate360 entity);

    /// <summary>
    /// Update Certificate360 instance.
    /// </summary>
    Task UpdateCertificate360(Certificate360 entity);

    /// <summary>
    /// Insert Certificate360History instance.
    /// </summary>
    Task<long> InsertCertificate360History(Certificate360History entity);

    /// <summary>
    /// Clears Certificate360 menu mapping.
    /// </summary>
    Task DeleteAllCertificate360Menus(long certificateID);

    /// <summary>
    /// Clears Certificate360 ingredient mapping.
    /// </summary>
    Task DeleteAllCertificate360Ingredients(long certificateID);

    /// <summary>
    /// Clears Certificate360 team mapping.
    /// </summary>
    Task DeleteAllCertificate360Teams(long certificateID);

    /// <summary>
    /// Maps certificate360 to menu.
    /// </summary>
    Task MapCertificate360ToMenus(long certificateID, IList<long> menuIDs);

    /// <summary>
    /// Maps certificate360 to ingredient.
    /// </summary>
    Task MapCertificate360ToIngredients(long certificateID, IList<long> menuIDs);

    /// <summary>
    /// Maps certificate360 to team.
    /// </summary>
    Task MapCertificate360ToTeams(long certificateID, IList<long> menuIDs);

    /// <summary>
    /// Insert auto renewal log after successful auto renewal trigger
    /// </summary>
    /// <param name="certificate360"></param>
    /// <returns></returns>
    Task<long> InsertAutoRenewalTriggerLog(string number, DateTimeOffset? expiresOn);
  }

  public class Certificate360Filter
  {
    public long[] IDs { get; set; }

    public long[] PremiseIDs { get; set; }
  }

  public class Certificate360IngredientFilter
  {
    public string Name { get; set; }

    public string SupplierName { get; set; }

    public string BrandName { get; set; }

    public string CertifyingBodyName { get; set; }
  }

}

