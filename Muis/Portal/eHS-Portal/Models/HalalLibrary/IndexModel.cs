using eHS.Portal.Model;
using System.Collections.Generic;

namespace eHS.Portal.Models.HalalLibrary
{
  public class IndexModel
  {
    public long totalData { get; set; }

    public List<HLIngredient> data { get; set; }
  }
}
