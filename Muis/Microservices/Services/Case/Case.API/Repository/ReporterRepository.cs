using Case.Model;
using Core.API.Repository;
using Dapper;
using System;
using System.Data;
using System.Threading.Tasks;

namespace Case.API.Repository
{
  public class ReporterRepository : IReporterRepository
  {
    readonly IUnitOfWork _unitOfWork;

    public ReporterRepository(IUnitOfWork unitOfWork)
    {
      _unitOfWork = unitOfWork;
    }

    public async Task<Guid> InsertReporter(Person person)
    {
      var param = new DynamicParameters();
      param.Add("@ReportedByName", person.Name);
      param.Add("@Out", dbType: DbType.Guid, direction: ParameterDirection.Output);

      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        "InsertReporter",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);

      return param.Get<Guid>("@Out");
    }
  }
}
