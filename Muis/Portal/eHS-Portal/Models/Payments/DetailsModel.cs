using eHS.Portal.DTO;
using System.Collections.Generic;

namespace eHS.Portal.Models.Payments
{
  public class DetailsModel
  {
    public long ID { get; set; }

    public Model.Payment Data { get; set; }

    public Model.Request Request { get; set; }

    public IList<Model.Notes> Notes { get; set; }
  }
}