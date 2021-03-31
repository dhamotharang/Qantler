using Core.API.Repository;
using Core.Model;
using Dapper;
using JobOrder.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace JobOrder.API.Repository
{
  public class PremiseRepository : IPremiseRepository
  {
    readonly IUnitOfWork _unitOfWork;

    public PremiseRepository(IUnitOfWork unitOfWork)
    {
      _unitOfWork = unitOfWork;
    }

    public async Task<Premise> GetPremiseByID(long id)
    {
      var param = new DynamicParameters();
      param.Add("@ID", id);

      return (await SqlMapper.QueryAsync<Premise>(_unitOfWork.Connection,
        "GetPremiseByID",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction)).FirstOrDefault();
    }

    public async Task InsertOrReplace(Premise premise)
    {
      var param = new DynamicParameters();
      param.Add("@ID", premise.ID);
      param.Add("@Name", premise.Name);
      param.Add("@IsLocal", premise.IsLocal);
      param.Add("@Type", premise.Type);
      param.Add("@Area", premise.Area);
      param.Add("@Schedule", premise.Schedule);
      param.Add("@BlockNo", premise.ID);
      param.Add("@UnitNo", premise.UnitNo);
      param.Add("@FloorNo", premise.FloorNo);
      param.Add("@BuildingName", premise.BuildingName);
      param.Add("@Address1", premise.Address1);
      param.Add("@Address2", premise.Address2);
      param.Add("@City", premise.City);
      param.Add("@Province", premise.Province);
      param.Add("@Country", premise.Country);
      param.Add("@Postal", premise.Postal);
      param.Add("@Longtitude", premise.Longitude);
      param.Add("@Latitude", premise.Latitude);
      param.Add("@Grade", premise.Grade);
      param.Add("@IsHighPriority", premise.IsHighPriority);

      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        "InsertOrReplacePremise",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);
    }
  }
}
