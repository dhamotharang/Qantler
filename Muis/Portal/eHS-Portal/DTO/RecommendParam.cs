using eHS.Portal.Model;
using System.Collections.Generic;

namespace eHS.Portal.DTO
{
  public class RecommendParam
  {
    public IList<Review> Reviews { get; set; }

    public Officer AssignedTo { get; set; }
  }
}
