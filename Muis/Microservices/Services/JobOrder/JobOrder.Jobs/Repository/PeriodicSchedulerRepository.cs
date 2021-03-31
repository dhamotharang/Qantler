using Core.Base.Repository;
using Dapper;
using JobOrder.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using JobOrder.Jobs.Model;

namespace JobOrder.Jobs.Repository
{
  public class PeriodicSchedulerRepository : IPeriodicSchedulerRepository
  {
    readonly IUnitOfWork _unitOfWork;

    public PeriodicSchedulerRepository(IUnitOfWork unitOfWork)
    {
      _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<PeriodicScheduler>> GetSchedulers()
    {
      return (await SqlMapper.QueryAsync<PeriodicScheduler>(_unitOfWork.Connection,
        "Job_GetSchedulers",
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction)).ToList();
    }

    public async Task<IList<PeriodicScheduler>> Select(PeriodicSchedulerOptions scheduler)
    {
      var param = new DynamicParameters();
      param.Add("@ID", scheduler.ID);
      param.Add("@PremiseID", scheduler.PremiseID);
      param.Add("@LastJobId", scheduler.LastJobID);
      param.Add("@Status", scheduler.Status);

      return (await SqlMapper.QueryAsync<PeriodicScheduler>(_unitOfWork.Connection,
        "SelectPeriodicScheduler",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction)).ToList();
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
