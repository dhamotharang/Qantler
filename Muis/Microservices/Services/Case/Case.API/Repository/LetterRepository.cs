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
  public class LetterRepository : ILetterRepository
  {
    readonly IUnitOfWork _unitOfWork;

    public LetterRepository(IUnitOfWork unitOfWork)
    {
      _unitOfWork = unitOfWork;
    }

    public async Task<List<LetterTemplate>> GetTemplate(LetterType type)
    {
      var param = new DynamicParameters();
      param.Add("@Type", type);

      return (await SqlMapper.QueryAsync<LetterTemplate>(_unitOfWork.Connection,
        "GetLetterTemplate",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction)).ToList();
    }

    public async Task UpdateTemplate(LetterTemplate template, Guid id, string userName)
    {
      var param = new DynamicParameters();
      param.Add("@ID", template.ID);
      param.Add("@Body", template.Body);

      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        "UpdateLetterTemplate",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);
    }

    public async Task<long> InsertLetter(Letter data)
    {
      var caseParam = new DynamicParameters();

      caseParam.Add("@Type", data.Type);
      caseParam.Add("@Body", data.Body);
      caseParam.Add("@EmailID", data.EmailID);
      caseParam.Add("@Status", data.Status);
      caseParam.Add("@Out", dbType: DbType.Int64, direction: ParameterDirection.Output);

      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        "InsertLetter",
        caseParam,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);

      var LetterID = caseParam.Get<long>("@Out");

      return LetterID;
    }

    public async Task<long> UpdateLetter(Letter data)
    {
      var caseParam = new DynamicParameters();

      caseParam.Add("@ID", data.ID);
      caseParam.Add("@Type", data.Type);
      caseParam.Add("@Body", data.Body);
      caseParam.Add("@EmailID", data.EmailID);
      caseParam.Add("@Status", data.Status);
      caseParam.Add("@Out", dbType: DbType.Int64, direction: ParameterDirection.Output);

      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        "UpdateLetter",
        caseParam,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);

      var LetterID = caseParam.Get<long>("@Out");

      return LetterID;
    }

    public async Task<Letter> GetLetterByID(long letterID)
    {
      var param = new DynamicParameters();

      param.Add("@ID", letterID);

      return (await SqlMapper.QueryAsync<Letter>(_unitOfWork.Connection,
        "GetLetterByID",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction)).FirstOrDefault();
    }
  }
}
