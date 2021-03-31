using Core.Base.Repository;
using Dapper;
using JobOrder.Jobs.Model;
using JobOrder.Jobs.Repository.Mappers;
using JobOrder.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobOrder.Jobs.Repository
{
  public class CertificateRepository : ICertificateRepository
  {
    readonly IUnitOfWork _unitOfWork;

    public CertificateRepository(IUnitOfWork unitOfWork)
    {
      _unitOfWork = unitOfWork;
    }

    public async Task<Certificate> Select(CertificateOptions certificate)
    {
      var param = new DynamicParameters();
      param.Add("@Number", certificate.Number);
      param.Add("@Status", certificate.Status);
      param.Add("@Scheme", certificate.Number);
      param.Add("@SubScheme", certificate.SubScheme);
      param.Add("@CustomerID", certificate.CustomerID);
      param.Add("@PremiseID", certificate.PremiseID);

      var mapper = new CertificateMapper();

      return (await SqlMapper.QueryAsync(_unitOfWork.Connection,
        "SelectCertificateFull",
        new[]
        {
          typeof(Certificate),
          typeof(Premise),
          typeof(Customer)
        },
        obj =>
        {
          Certificate ce = obj[0] as Certificate;
          Premise pr = obj[1] as Premise;
          Customer cu = obj[2] as Customer;
          return mapper.Map(ce, pr, cu);
        },
        param,
        splitOn: "PremiseID,CustomerID",
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction)).FirstOrDefault();
    }
  }
}
