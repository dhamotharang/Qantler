using Case.Model;
using Core.API.Repository;
using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Case.API.Repository
{
  public class CertificateRepository : ICertificateRepository
  {
    readonly IUnitOfWork _unitOfWork;

    public CertificateRepository(IUnitOfWork unitOfWork)
    {
      _unitOfWork = unitOfWork;
    }

    public async Task InsertCertificate(Certificate certificate)
    {
      var param = new DynamicParameters();
      param.Add("@Number", certificate.Number);
      param.Add("@Scheme", certificate.Scheme);
      param.Add("@SubScheme", certificate.SubScheme);
      param.Add("@IssuedOn", certificate.IssuedOn);
      param.Add("@StartsFrom", certificate.StartsFrom);
      param.Add("@ExpiresOn", certificate.ExpiresOn);
      param.Add("@SerialNo", certificate.SerialNo);
      param.Add("@PremiseID", certificate.PremiseID);
      param.Add("@CaseID", certificate.CaseID);
      param.Add("@Status", certificate.Status);
      param.Add("@SuspendedUntil", certificate.SuspendedUntil);

      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        "InsertCertificate",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);
    }

    public async Task UpdateCertificate(Certificate certificate)
    {
      var param = new DynamicParameters();
      param.Add("@Number", certificate.Number);
      param.Add("@Scheme", certificate.Scheme);
      param.Add("@SubScheme", certificate.SubScheme);
      param.Add("@IssuedOn", certificate.IssuedOn);
      param.Add("@StartsFrom", certificate.StartsFrom);
      param.Add("@ExpiresOn", certificate.ExpiresOn);
      param.Add("@SerialNo", certificate.SerialNo);
      param.Add("@PremiseID", certificate.PremiseID);
      param.Add("@CaseID", certificate.CaseID);
      param.Add("@Status", certificate.Status);
      param.Add("@ID", certificate.ID);
      param.Add("@SuspendedUntil", certificate.SuspendedUntil);

      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        "UpdateCertificate",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);
    }

    public async Task<IList<Certificate>> GetCertificate(long? caseID)
    {
      var param = new DynamicParameters();
      param.Add("@CaseID", caseID);

      return (await SqlMapper.QueryAsync(_unitOfWork.Connection,
       "GetCertificate",
       new[]
       {
            typeof(Certificate)
       },
       obj =>
       {
         return obj[0] as Certificate;
       },
       param,
       commandType: CommandType.StoredProcedure,
       transaction: _unitOfWork.Transaction)).Distinct().ToList();
    }
  }
}
