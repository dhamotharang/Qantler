using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Model;
using Microsoft.AspNetCore.Mvc;
using Request.API.Repository;
using Request.API.Services;
using Request.Model;

namespace Request.API.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class CertificateController : ControllerBase
  {
    readonly ICertificateService _certificateService;

    public CertificateController(ICertificateService certificateService)
    {
      _certificateService = certificateService;
    }

    [HttpGet]
    [Route("list")]
    public async Task<IList<Certificate>> UpdateCertificateBatchStatus(
      [FromQuery] long[] ids)
    {
      return await _certificateService.CertificateFilter(new CertificateFilter
      {
        IDs = ids
      });
    }

    [Route("batch/list")]
    public async Task<IList<CertificateBatch>> List(DateTimeOffset from,
      [FromQuery] CertificateBatchStatus[] status, bool includeAll = false)
    {
      return await _certificateService.BatchFilter(new CertificateBatchFilter
      {
        From = from,
        Status = status
      }, includeAll);
    }

    [Route("batch/{id:int}")]
    public async Task<CertificateBatch> GetCertificateBatch(long id, bool includeAll = false)
    {
      return await _certificateService.GetCertificateBatchByID(id, includeAll);
    }

    [HttpPost]
    [Route("batch/{id:int}/comment")]
    public async Task<Comment> AddComment(long id, string text, Guid userID, string userName)
    {
      return await _certificateService.AddComment(id, text, new Officer
      {
        ID = userID,
        Name = userName
      });
    }

    [HttpGet]
    [Route("batch/{id:int}/comments")]
    public async Task<IList<Comment>> GetCertificateBatchComments(long id)
    {
      return await _certificateService.GetCertificateBatchComments(id);
    }

    [HttpPost]
    [Route("batch/acknowledge")]
    public async Task<string> AcknowledgeBatch([FromBody] long[] ids, Guid userID, string userName)
    {
      await _certificateService.AcknowledgeCertificateBatch(ids, new Officer
      {
        ID = userID,
        Name = userName
      });

      return "Ok";
    }

    [HttpPost]
    [Route("batch/{id:int}/link/file")]
    public async Task<string> GetCertificateBatch(long id, Guid fileID)
    {
      await _certificateService.MapCertificateBatchFile(id, fileID);
      return "Ok";
    }

    [HttpPut]
    [Route("batch/{id:int}/status")]
    public async Task<string> UpdateCertificateBatchStatus(long id, CertificateBatchStatus status)
    {
      await _certificateService.UpdateCertificateBatchStatus(id, status);
      return "Ok";
    }

    [HttpGet]
    [Route("deliverylist")]
    public async Task<IList<Certificate>> GetDeliveryCertificates(string customerCode,
      string customerName, string postal, string premise, string serialNo,
      DateTimeOffset? issuedOnFrom, DateTimeOffset? issuedOnTo,
     [FromQuery] CertificateDeliveryStatus[] status)
    {
      return await _certificateService.CertDeliveryFilter(new CertificateDeliveryFilter
      {
        CustomerCode = customerCode,
        CustomerName = customerName,
        IssuedOnFrom = issuedOnFrom,
        IssuedOnTo = issuedOnTo,
        Postal = postal,
        Premise = premise,
        SerialNo = serialNo,
        Status = status
      });
    }


    [HttpPost]
    [Route("deliver")]
    public async Task<string> UpdateCertificateDeliveryStatus
      ([FromBody] long[] IDs)
    {
      await _certificateService.UpdateCertificateDeliveryStatus(IDs);
      return "Ok";
    }
  }
}
