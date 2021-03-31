using Core.API.Repository;
using Dapper;
using JobOrder.API.Repository.Mappers;
using JobOrder.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace JobOrder.API.Repository
{

  public class MasterRepository : IMasterRepository
  {
    readonly IUnitOfWork _unitOfWork;

    public MasterRepository(IUnitOfWork unitOfWork)
    {
      _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<Master>> GetMasterList(MasterType type)
    {
      var param = new DynamicParameters();
      param.Add("@Type", type);

      var mapper = new MasterMapper();

      return (await SqlMapper.QueryAsync(_unitOfWork.Connection,
        "GetMaster",
        new[]
        {
          typeof(Master)
        },
        obj =>
        {
          var history = obj[0] as Master;
          return mapper.Map(history);
        },
        param,
        splitOn: "Type,ID,Value,ParentID,CreatedOn,ModifiedOn,IsDeleted",
        commandType: CommandType.StoredProcedure));
    }

    public async Task InsertMaster(Master data)
    {
      var masterParam = new DynamicParameters();
      masterParam.Add("@Type", data.Type);
      masterParam.Add("@ID", Guid.NewGuid());
      masterParam.Add("@Value", data.Value);

      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        "InsertMaster",
        masterParam,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);
    }

    public async Task UpdateMaster(Master data)
    {
      var masterParam = new DynamicParameters();
      masterParam.Add("@Type", data.Type);
      masterParam.Add("@ID", data.ID);
      masterParam.Add("@Value", data.Value);

      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        "UpdateMaster",
        masterParam,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);
    }

    public async Task DeleteMaster(Guid id)
    {
      var masterParam = new DynamicParameters();
      masterParam.Add("@ID", id);

      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        "DeleteMaster",
        masterParam,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);
    }
  }
}
