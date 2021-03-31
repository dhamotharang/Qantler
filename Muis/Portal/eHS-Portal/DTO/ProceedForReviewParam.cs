using eHS.Portal.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eHS.Portal.DTO
{
  public class ProceedForReviewParam
  {
    public long RequestID { get; set; }
    public Officer AssignOfficer { get; set; }
  }
}
