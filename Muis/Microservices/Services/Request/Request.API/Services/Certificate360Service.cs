using Core.API;
using Core.API.Provider;
using Core.Model;
using Request.API.Repository;
using Request.API.Services.Commands.Certificate360;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Request.API.Services
{
  public class Certificate360Service : TransactionalService, ICertificate360Service
  {

    public Certificate360Service(IDbConnectionProvider connectionProvider)
      : base(connectionProvider)
    {
    }

    public async Task<IList<Model.Certificate360>> GetCertificatesForRenewal
      (Scheme scheme, SubScheme? subScheme)
    {
      return await Execute(new GetCertForRenewalCommand(scheme, subScheme));
    }

    public async Task<Model.Certificate360> GetCertificateByCertNo(string CertificateNo)
    {
      return await Execute(new GetCertByCertNoCommand(CertificateNo));
    }

    public async Task<Model.Certificate360> GetCertificateByCertID(long ID)
    {
      return await Execute(new GetCertByCertIDCommand(ID));
    }

    public async Task<IList<Model.Certificate360History>> GetCertHistory(long ID)
    {
      return await Execute(new GetCertHistoryByCertIDCommand(ID));
    }

    public async Task<IList<Model.Menu>> GetCertMenus(long ID)
    {
      return await Execute(new GetCertMenuByCertIDCommand(ID));
    }

    public async Task<IList<Model.Ingredient>> GetCertIngredients(long ID)
    {
      return await Execute(new GetCertIngredientByCertIDCommand(ID));
    }

    public async Task<IList<Model.Certificate360>> GetCertWithIngredient
      (Certificate360IngredientFilter filter)
    {
      return await Execute(new GetCertWithIngredientCommand(filter));
    }

    public async Task<long> InsertAutoRenewalTriggerLog(string Number, DateTimeOffset? ExpiresOn)
    {
      return await Execute(new InsertAutoRenewTrigLogCommand(Number, ExpiresOn));
    }

    public async Task<IList<Model.Certificate360>> GetCertificates(Certificate360Filter filter)
    {
      return await Execute(new SelectCertCommand(filter));
    }
  }
}
