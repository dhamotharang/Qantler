using Core.Model;
using HalalLibrary.API.Models;
using HalalLibrary.Model;
using System;
using System.Collections.Generic;

namespace HalalLibrary.API.Repository.Mappers
{
  public class HalalLibraryMapper
  {
    IDictionary<long, Ingredient> _ingredientCache = new Dictionary<long, Ingredient>();
    halalLibraryList _halalLibrary = new halalLibraryList();

    public halalLibraryList Map(long total,
      Ingredient ingredient = null,
      Supplier supplier = null,
      CertifyingBody certifyingBody = null,
      Officer officer = null)
    {
      if (_halalLibrary.totalData == 0)
      {
        _halalLibrary.totalData = total;
      }

      Ingredient outIngredient = null;
      if ((ingredient?.ID ?? 0) > 0
         && !_ingredientCache.TryGetValue(ingredient.ID, out outIngredient))
      {
        _ingredientCache[ingredient.ID] = ingredient;

        if (_halalLibrary.data == null)
        {
          _halalLibrary.data = new List<Ingredient>();
        }

        if ((supplier?.ID ?? 0) > 0)
        {
          ingredient.Supplier = supplier;
        }

        if ((certifyingBody?.ID ?? 0) > 0)
        {
          ingredient.CertifyingBody = certifyingBody;
        }

        if (!string.IsNullOrEmpty(officer.Name))
        {
          ingredient.VerifiedBy = officer;
        }

        _halalLibrary.data.Add(ingredient);
      }

      return _halalLibrary;
    }
  }
}
