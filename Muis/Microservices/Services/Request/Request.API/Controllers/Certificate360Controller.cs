using Core.Model;
using Microsoft.AspNetCore.Mvc;
using Request.API.Repository;
using Request.API.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Request.API.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class Certificate360Controller : ControllerBase
  {
    readonly ICertificate360Service _certificate360Service;

    public Certificate360Controller(ICertificate360Service certificate360Service)
    {
      _certificate360Service = certificate360Service;
    }

    [HttpGet]
    [Route("renewalcerts")]
    public async Task<IList<Model.Certificate360>> Get(Scheme scheme, SubScheme? subScheme)
    {
      return await _certificate360Service.GetCertificatesForRenewal(scheme, subScheme);
    }

    [HttpGet]
    [Route("{id:int}")]
    public async Task<Model.Certificate360> Get(long id)
    {
      return await _certificate360Service.GetCertificateByCertID(id);
    }

    [HttpGet]
    [Route("{id:int}/history")]
    public async Task<IList<Model.Certificate360History>> GetHistory(long id)
    {
      return await _certificate360Service.GetCertHistory(id);
    }

    [HttpGet]
    [Route("{id:int}/menu")]
    public async Task<IList<Model.Menu>> GetMenu(long id)
    {
      return await _certificate360Service.GetCertMenus(id);
    }

    [HttpGet]
    [Route("{id:int}/ingredient")]
    public async Task<IList<Model.Ingredient>> GetIngredient(long id)
    {
      return await _certificate360Service.GetCertIngredients(id);
    }

    [HttpGet]
    [Route("with-ingredient")]
    public async Task<IList<Model.Certificate360>> GetWithIngredient(string name,
      string brand,
      string supplier,
      string certifyingBody)
    {
      return await _certificate360Service.GetCertWithIngredient(new Certificate360IngredientFilter
      {
        Name = name,
        BrandName = brand,
        SupplierName = supplier,
        CertifyingBodyName = certifyingBody
      });
    }

    [HttpPut]
    [Route("insertrenew")]
    public async Task<string> InsertAutoRenewTriggerLog(string Number, DateTimeOffset? ExpiresOn)
    {
      await _certificate360Service.InsertAutoRenewalTriggerLog(Number, ExpiresOn);
      return "okay";
    }

    [HttpGet]
    [Route("query")]
    public async Task<IList<Model.Certificate360>> Query(
      [FromQuery] long[] ids = null,
      [FromQuery] long[] premiseIDs = null)
    {
      return await _certificate360Service.GetCertificates(new Certificate360Filter
      {
        IDs = ids,
        PremiseIDs = premiseIDs
      });
    }
  }
}
