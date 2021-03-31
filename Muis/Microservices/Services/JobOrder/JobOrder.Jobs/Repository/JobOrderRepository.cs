using Core.Base.Repository;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobOrder.Jobs.Repository
{
  public class JobOrderRepository : IJobOrderRepository
  {
    readonly IUnitOfWork _unitOfWork;

    public JobOrderRepository(IUnitOfWork unitOfWork)
    {
      _unitOfWork = unitOfWork;
    }
    public async Task<long> InsertJobOrder(JobOrder.Model.JobOrder model)
    {
      var param = new DynamicParameters();
      param.Add("@RefID", model.RefID);
      param.Add("@Type", model.Type);
      param.Add("@Status", model.Status);
      param.Add("@Notes", model.Notes);
      param.Add("@CustomerID", model.Customer.ID);
      param.Add("@CustomerName", model.Customer.Name);
      param.Add("@CustomerCode", model.Customer.Code);
      param.Add("@OfficerID", model.Officer?.ID);
      param.Add("@OfficerName", model.Officer?.Name);
      param.Add("@TargetDate", model.TargetDate);
      param.Add("@ScheduledOn", model.ScheduledOn);
      param.Add("@ScheduledOnTo", model.ScheduledOnTo);
      param.Add("@ID", dbType: DbType.Int64, direction: ParameterDirection.Output);

      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        "InsertJobOrder",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);

      var result = param.Get<long>("@ID");

      if (model.LineItems?.Any() ?? false)
      {
        foreach (var lineItem in model.LineItems)
        {
          var lparam = new DynamicParameters();
          lparam.Add("@Scheme", lineItem.Scheme);
          lparam.Add("@SubScheme", lineItem.SubScheme);
          lparam.Add("@ChecklistHistoryID", lineItem.ChecklistHistoryID);
          lparam.Add("@JobID", result);
          lparam.Add("@ID", dbType: DbType.Int64, direction: ParameterDirection.Output);

          await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
            "InsertJobOrderLineItem",
            lparam,
            commandType: CommandType.StoredProcedure,
            transaction: _unitOfWork.Transaction);
        }
      }

      if (model.Premises?.Any() ?? false)
      {
        var premises = new DataTable();
        premises.Columns.Add("A", typeof(long));
        premises.Columns.Add("B", typeof(long));

        foreach (var premise in model.Premises)
        {
          var premParam = new DynamicParameters();
          premParam.Add("@ID", premise.ID);
          premParam.Add("@IsLocal", premise.IsLocal);
          premParam.Add("@Name", premise.Name);
          premParam.Add("@Type", premise.Type);
          premParam.Add("@Area", premise.Area);
          premParam.Add("@Schedule", premise.Schedule);
          premParam.Add("@BlockNo", premise.BlockNo);
          premParam.Add("@UnitNo", premise.UnitNo);
          premParam.Add("@FloorNo", premise.FloorNo);
          premParam.Add("@BuildingName", premise.BuildingName);
          premParam.Add("@Address1", premise.Address1);
          premParam.Add("@Address2", premise.Address2);
          premParam.Add("@City", premise.City);
          premParam.Add("@Province", premise.Province);
          premParam.Add("@Country", premise.Country);
          premParam.Add("@Postal", premise.Postal);
          premParam.Add("@Latitude", premise.Latitude);
          premParam.Add("@Longtitude", premise.Longitude);
          premParam.Add("@Grade", null);
          premParam.Add("@IsHighPriority", null);

          await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
            "InsertOrReplacePremise",
            premParam,
            commandType: CommandType.StoredProcedure,
            transaction: _unitOfWork.Transaction);

          premises.Rows.Add(result, premise.ID);
        }

        var premisesParam = new DynamicParameters();
        premisesParam.Add("@IDMappingType", premises.AsTableValuedParameter("dbo.IDMappingType"));

        await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
          "InsertJobOrderPremises",
          premisesParam,
          commandType: CommandType.StoredProcedure,
          transaction: _unitOfWork.Transaction);
      }

      return result;
    }

  }
}
