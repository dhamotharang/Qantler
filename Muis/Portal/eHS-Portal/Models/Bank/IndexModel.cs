using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eHS.Portal.Models.Bank
{
  public class IndexModel
  {
    public IList<Model.Bank> Dataset { get; set; }

    public IList<Model.Master> Banks { get; set; }
  }
}
