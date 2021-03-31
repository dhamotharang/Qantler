using Core.Model;
using JobOrder.API.Models;
using JobOrder.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobOrder.API.Services
{
  public interface IPeriodicInspectionService
  {
    /// <summary>
    /// Certificate issued event for job order
    /// </summary>    
    Task<Unit> OnCertificateIssued(OnCertificateIssuedParam eventParam);
  }
}
