using Core.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobOrder.Model
{
  public class Premise : BasePremise
  {
    public int? Grade { get; set; }

    public bool IsHighPriority { get; set; }
  }
}
