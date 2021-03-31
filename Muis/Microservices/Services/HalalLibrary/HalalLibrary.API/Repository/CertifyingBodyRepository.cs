using Core.API.Repository;
using Dapper;
using HalalLibrary.Model;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace HalalLibrary.API.Repository
{
  public class CertifyingBodyRepository : ICertifyingBodyRepository
  {
    readonly IUnitOfWork _unitOfWork;

    public CertifyingBodyRepository(IUnitOfWork unitOfWork)
    {
      _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<CertifyingBody>> Select()
    {
      return (await SqlMapper.QueryAsync<CertifyingBody>(_unitOfWork.Connection,
        "SelectCertifyingBody",
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction)).Distinct().ToList();
    }

    public async Task<CertifyingBody> GetCertifyingBodyByName(string name)
    {
      var selectCertifyingBody = $"SELECT * FROM CertifyingBody c " +
        $"WHERE c.[IsDeleted] = 0 AND c.[Name] LIKE '%{name}%'";
      return (await _unitOfWork.Connection.QueryAsync<CertifyingBody>(selectCertifyingBody,
        transaction: _unitOfWork.Transaction)).FirstOrDefault();
    }

    public async Task<long> InsertCertifyingBody(CertifyingBody data)
    {

      var certifyingBodyParam = new DynamicParameters();
      certifyingBodyParam.Add("@Name", data.Name);
      certifyingBodyParam.Add("@Status", data.Status);
      certifyingBodyParam.Add("@ID", dbType: DbType.Int64, direction: ParameterDirection.Output);

      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        "InsertCertifyingBody",
        certifyingBodyParam,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);

      return certifyingBodyParam.Get<long>("@ID");
    }
  }
}
