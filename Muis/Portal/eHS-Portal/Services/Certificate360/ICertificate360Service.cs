using eHS.Portal.Client;
using eHS.Portal.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eHS.Portal.Services.Certificate360
{
  public interface ICertificate360Service
  {
    /// <summary>
    /// Get certificate 360 by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<Model.Certificate360> GetByID(long id);

    /// <summary>
    /// Get certificate 360 History by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<IList<Certificate360History>> GetHistoryByID(long id);

    /// <summary>
    /// Get certificate 360 Menu by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<IList<Menu>> GetMenuByID(long id);

    /// <summary>
    /// Get certificate 360 Ingredient by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<IList<Ingredient>> GetIngredientByID(long id);

    /// <summary>
    /// Get certificate 360 With Ingredient
    /// </summary>
    /// <param name="options"></param>
    /// <returns></returns>
    Task<IList<Model.Certificate360>> GetWithIngredient
      (Certificate360IngredientOptions options);
  }
}
