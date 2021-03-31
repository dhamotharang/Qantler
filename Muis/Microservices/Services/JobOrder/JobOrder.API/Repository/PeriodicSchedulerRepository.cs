using Core.API.Repository;
using Dapper;
using JobOrder.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace JobOrder.API.Repository
{
  public class PeriodicSchedulerRepository : IPeriodicSchedulerRepository
  {
    readonly IUnitOfWork _unitOfWork;

    public PeriodicSchedulerRepository(IUnitOfWork unitOfWork)
    {
      _unitOfWork = unitOfWork;
    }

    public async Task<PeriodicScheduler> GetPeriodicSchedulerByPremise(long premiseID)
    {
      var param = new DynamicParameters();
      param.Add("@PremiseId", premiseID);

      return (await SqlMapper.QueryAsync<PeriodicScheduler>(_unitOfWork.Connection,
        "GetPeriodicSchedulerByPremise",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction)).FirstOrDefault();
    }

    public async Task InsertOrReplace(PeriodicScheduler scheduler)
    {
      var param = new DynamicParameters();
      param.Add("@PremiseID", scheduler.PremiseID);
      param.Add("@LastJobID", scheduler.LastJobID);
      param.Add("@LastScheduledOn", scheduler.LastScheduledOn);
      param.Add("@NextTargetInspection", scheduler.NextTargetInspection);
      param.Add("@Status", scheduler.Status);      

      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        "InsertOrReplacePeriodicScheduler",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);
    }
  }
}
