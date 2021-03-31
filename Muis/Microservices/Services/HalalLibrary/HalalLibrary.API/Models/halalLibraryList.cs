using HalalLibrary.Model;
using System.Collections.Generic;

namespace HalalLibrary.API.Models
{
  public class halalLibraryList
  {
    public long totalData { get; set; }

    public List<Ingredient> data { get; set; }
  }
}
