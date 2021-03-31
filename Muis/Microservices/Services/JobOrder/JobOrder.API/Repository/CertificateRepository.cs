using Core.API.Repository;
using Core.Model;
using Dapper;
using JobOrder.API.Models;
using JobOrder.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace JobOrder.API.Repository
{
  public class CertificateRepository : ICertificateRepository
  {
    readonly IUnitOfWork _unitOfWork;

    public CertificateRepository(IUnitOfWork unitOfWork)
    {
      _unitOfWork = unitOfWork;
    }

    public async Task InsertOrReplace(Certificate certificate)
    {
      var param = new DynamicParameters();
      param.Add("@Number", certificate.Number);
      param.Add("@Status", certificate.Status);
      param.Add("@Scheme", certificate.Scheme);
      param.Add("@SubScheme", certificate.SubScheme);
      param.Add("@IssuedOn", certificate.IssuedOn);
      param.Add("@StartsFrom", certificate.StartsFrom);
      param.Add("@ExpiresOn", certificate.ExpiresOn);
      param.Add("@CustomerID", certificate.CustomerID);
      param.Add("@PremiseID", certificate.PremiseID);
      param.Add("@IsDeleted", certificate.IsDeleted);

      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        "InsertOrReplaceCertificate",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);
    }

    public async Task DeleteCertificateByPremise(long premiseID, Scheme schme, SubScheme? subScheme)
    {
      var param = new DynamicParameters();
      param.Add("@PremiseID", premiseID);
      param.Add("@Scheme", schme);
      param.Add("@SubScheme", subScheme);

      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        "DeleteCertificateByPremise",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);
    }

    public async Task<IEnumerable<Certificate>> Select(CertificateOptions options)
    {
      var param = new DynamicParameters();
      param.Add("@Number", options.Number);
      param.Add("@Status", options.Status);
      param.Add("@Scheme", options.Scheme);
      param.Add("@SubScheme", options.SubScheme);
      param.Add("@CustomerID", options.CustomerID);
      param.Add("@PremiseID", options.PremiseID);
      param.Add("@IsDeleted", options.IsDeleted);

      return (await SqlMapper.QueryAsync<Certificate>(_unitOfWork.Connection,
        "SelectCertificateBasic",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction)).ToList();
    }
  }
}
