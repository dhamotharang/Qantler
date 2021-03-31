using Case.Model;
using Core.API.Repository;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Case.API.Repository
{
  public class OffenderRepository : IOffenderRepository
  {
    readonly IUnitOfWork _unitOfWork;

    public OffenderRepository(IUnitOfWork unitOfWork)
    {
      _unitOfWork = unitOfWork;
    }

    public async Task InsertOffender(Offender offender)
    {
      var param = new DynamicParameters();
      param.Add("@OffenderName", offender.Name);
      param.Add("@OffenderID", offender.ID);

      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        "InsertOffender",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);
    }

    public async Task<Offender> GetOffenderByID(Guid id)
    {
      var param = new DynamicParameters();
      param.Add("@ID", id);

      return (await SqlMapper.QueryAsync(_unitOfWork.Connection,
       "GetOffenderByID",
       new[]
       {
            typeof(Offender)
       },
       obj =>
       {
         return obj[0] as Offender;
       },
       param,
       commandType: CommandType.StoredProcedure,
       transaction: _unitOfWork.Transaction)).Distinct().FirstOrDefault();
    }
  }
}
