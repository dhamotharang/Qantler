using System;

namespace eHS.Portal.Models.Request
{
  public class DetailsModel
  {
    public long ID { get; set; }

    public Guid CurrentUserID { get; set; }

    public Model.Request Data { get; set; }
  }
}
