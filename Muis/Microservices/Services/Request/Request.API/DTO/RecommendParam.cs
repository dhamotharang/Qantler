using Core.Model;
using Request.Model;
using System.Collections.Generic;

namespace Request.API.DTO
{
  public class RecommendParam
  {
    public IList<Review> reviews { get; set; }

    public Officer AssignedTo { get; set; }
  }
}
