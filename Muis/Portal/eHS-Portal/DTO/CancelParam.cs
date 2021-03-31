using System;
using eHS.Portal.Model;

namespace eHS.Portal.DTO
{
  public class CancelParam
  {
    public Master Reason { get; set; }

    public string Notes { get; set; }
  }
}
