using System;
using JobOrder.Model;

namespace JobOrder.API.Models
{
  public class CancelParam
  {
    public Master Reason { get; set; }

    public string Notes { get; set; }
  }
}
