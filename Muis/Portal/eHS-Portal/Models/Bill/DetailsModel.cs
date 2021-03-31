using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eHS.Portal.Models.Bill
{  
  public class DetailsModel
  {
    public long ID { get; set; }

    public Model.Bill Data { get; set; }
  }
}
