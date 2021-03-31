using Core.API.Repository;
using Core.Model;
using Dapper;
using JobOrder.API.Models;
using JobOrder.API.Repository.Mappers;
using JobOrder.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace JobOrder.API.Repository
{
  public class ContactInfoRepository : IContactInfoRepository
  {
    readonly IUnitOfWork _unitOfWork;

    public ContactInfoRepository(IUnitOfWork unitOfWork)
    {
      _unitOfWork = unitOfWork;
    }

    public async Task<long> InsertContactInfo(ContactInfo info)
    {
      var param = new DynamicParameters();
      param.Add("@Type", info.Type);
      param.Add("@Value", info.Value);
      param.Add("@IsPrimary", info.IsPrimary);
      param.Add("@ID", dbType: DbType.Int64, direction: ParameterDirection.Output);

      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        "InsertContactInfo",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);

      return param.Get<long>("@ID");
    }

    public async Task<IList<ContactInfo>> Select(Guid personID, ContactInfoType type)
    {
      var param = new DynamicParameters();
      param.Add("@PersonID", personID);
      param.Add("@Type", type);

      return (await SqlMapper.QueryAsync<ContactInfo>(_unitOfWork.Connection,
        "SelectContactInfo",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction)).ToList();
    }

    public async Task UpdateContactInfo(ContactInfo info)
    {
      var param = new DynamicParameters();
      param.Add("@Type", info.Type);
      param.Add("@Value", info.Value);
      param.Add("@IsPrimary", info.IsPrimary);
      param.Add("@ID", dbType: DbType.Int64, direction: ParameterDirection.Output);

      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        "UpdateContactInfo",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);
    }
  }
}
