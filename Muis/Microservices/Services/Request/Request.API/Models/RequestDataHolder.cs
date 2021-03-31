using Request.Model;
using System.Collections.Generic;

namespace Request.API.Models
{
  public class RequestDataHolder
  {
    public RequestStatus NewStatus { get; set; }

    public RequestStatus OldStatus { get; set; }

    public Model.Request Request { get; set; }

    public IList<Certificate> Certificates { get; set; }
  }
}
