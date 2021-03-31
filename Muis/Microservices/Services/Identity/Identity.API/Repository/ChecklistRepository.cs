using Core.API;
using Core.API.Exceptions;
using Core.API.Repository;
using Core.Model;
using Dapper;
using Identity.API.Repository.Mappers;
using Request.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.API.Repository
{
  public class ChecklistRepository : IChecklistRepository
  {
    readonly IUnitOfWork _unitOfWork;

    public ChecklistRepository(IUnitOfWork unitOfWork)
    {
      _unitOfWork = unitOfWork;
    }

    public async Task<ChecklistHistory> GetChecklistHistoryByID(long id)
    {
      var mapper = new ChecklistMapper();

      var param = new DynamicParameters();
      param.Add("@ID", id);

      return (await SqlMapper.QueryAsync(_unitOfWork.Connection,
        "GetChecklistHistoryByID",
        new[]
        {
          typeof(ChecklistHistory),
          typeof(ChecklistCategory),
          typeof(ChecklistItem)
        },
        obj =>
        {
          var history = obj[0] as ChecklistHistory;
          var category = obj[1] as ChecklistCategory;
          var item = obj[2] as ChecklistItem;

          return mapper.Map(history, category, item);
        },
        param,
        splitOn: "ID,CategoryID,ItemID",
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction))?.FirstOrDefault();
    }

    public async Task<ChecklistHistory> GetLatestChecklist(Scheme scheme)
    {
      var mapper = new ChecklistMapper();

      var param = new DynamicParameters();
      param.Add("@Scheme", scheme);

      return (await SqlMapper.QueryAsync(_unitOfWork.Connection,
        "GetLatestChecklist",
        new[]
        {
          typeof(ChecklistHistory),
          typeof(ChecklistCategory),
          typeof(ChecklistItem)
        },
        obj =>
        {
          var history = obj[0] as ChecklistHistory;
          var category = obj[1] as ChecklistCategory;
          var item = obj[2] as ChecklistItem;

          return mapper.Map(history, category, item);
        },
        param,
        splitOn: "ID,CategoryID,ItemID",
        commandType: System.Data.CommandType.StoredProcedure))?.FirstOrDefault();
    }

    public async Task<IEnumerable<ChecklistHistory>> GetChecklistHistoryByScheme(int id)
    {
      var mapper = new ChecklistMapper();

      var param = new DynamicParameters();
      param.Add("@ID", id);

      return (await SqlMapper.QueryAsync(_unitOfWork.Connection,
        "GetChecklistHistoryByScheme",
        new[]
        {
          typeof(ChecklistHistory),
          typeof(ChecklistCategory),
          typeof(ChecklistItem)
        },
        obj =>
        {
          var history = obj[0] as ChecklistHistory;
          var category = obj[1] as ChecklistCategory;
          var item = obj[2] as ChecklistItem;

          return mapper.Map(history, category, item);
        },
        param,
        splitOn: "ID,CategoryID,ItemID",
        commandType: System.Data.CommandType.StoredProcedure)).Distinct();
    }

    public async Task<bool> InsertChecklist(ChecklistHistory checklist)
    {
      var checklistHistoryParam = new DynamicParameters();
      checklistHistoryParam.Add("@Scheme", checklist.Scheme);
      checklistHistoryParam.Add("@Version", checklist.Version);
      checklistHistoryParam.Add("@CreatedBy", checklist.CreatedBy);
      checklistHistoryParam.Add("@EffectiveFrom", checklist.EffectiveFrom);
      checklistHistoryParam.Add("@ID", dbType: DbType.Int64, direction: ParameterDirection.Output);

      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
       "InsertChecklistHistory",
       checklistHistoryParam,
       commandType: CommandType.StoredProcedure,
       transaction: _unitOfWork.Transaction);

      checklist.ID = checklistHistoryParam.Get<long>("@ID");

      //Insert checklist category
      foreach (var checklistCategory in checklist.Categories)
      {

        var checklistCategoryParam = new DynamicParameters();
        checklistCategoryParam.Add("@Index", checklistCategory.Index);
        checklistCategoryParam.Add("@Text", checklistCategory.Text);
        checklistCategoryParam.Add("@HistoryID", checklist.ID);
        checklistCategoryParam.Add("@ID", dbType: DbType.Int64, direction: ParameterDirection.Output);

        await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
          "InsertChecklistCategory",
          checklistCategoryParam,
          commandType: CommandType.StoredProcedure,
          transaction: _unitOfWork.Transaction);

        checklistCategory.ID = checklistCategoryParam.Get<long>("@ID");

        //Insert checklist items
        foreach (var checklistCategoryItem in checklistCategory.Items)
        {

          var checklistItemParam = new DynamicParameters();
          checklistItemParam.Add("@Index", checklistCategoryItem.Index);
          checklistItemParam.Add("@Text", checklistCategoryItem.Text);
          checklistItemParam.Add("@CategoryID", checklistCategory.ID);
          checklistItemParam.Add("@Notes", checklistCategoryItem.Notes);
          checklistItemParam.Add("@ID", dbType: DbType.Int64, direction: ParameterDirection.Output);

          await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
            "InsertChecklistItem",
            checklistItemParam,
            commandType: CommandType.StoredProcedure,
            transaction: _unitOfWork.Transaction);
        }
      }
      return true;
    }

    public async Task<bool> UpdateChecklist(ChecklistHistory checklist)
    {
      var checklistHistoryParam = new DynamicParameters();
      checklistHistoryParam.Add("@ID", checklist.ID);
      checklistHistoryParam.Add("@Scheme", checklist.Scheme);
      checklistHistoryParam.Add("@Version", checklist.Version);
      checklistHistoryParam.Add("@EffectiveFrom", checklist.EffectiveFrom);

      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
       "UpdateChecklistHistory",
       checklistHistoryParam,
       commandType: CommandType.StoredProcedure,
       transaction: _unitOfWork.Transaction);

      //Insert checklist category
      foreach (var checklistCategory in checklist.Categories)
      {

        var checklistCategoryParam = new DynamicParameters();
        checklistCategoryParam.Add("@Index", checklistCategory.Index);
        checklistCategoryParam.Add("@Text", checklistCategory.Text);
        checklistCategoryParam.Add("@HistoryID", checklist.ID);
        checklistCategoryParam.Add("@ID", dbType: DbType.Int64, direction: ParameterDirection.Output);

        await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
          "InsertChecklistCategory",
          checklistCategoryParam,
          commandType: CommandType.StoredProcedure,
          transaction: _unitOfWork.Transaction);

        checklistCategory.ID = checklistCategoryParam.Get<long>("@ID");

        //Insert checklist items
        foreach (var checklistCategoryItem in checklistCategory.Items)
        {

          var checklistItemParam = new DynamicParameters();
          checklistItemParam.Add("@Index", checklistCategoryItem.Index);
          checklistItemParam.Add("@Text", checklistCategoryItem.Text);
          checklistItemParam.Add("@CategoryID", checklistCategory.ID);
          checklistItemParam.Add("@Notes", checklistCategoryItem.Notes);
          checklistItemParam.Add("@ID", dbType: DbType.Int64, direction: ParameterDirection.Output);

          await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
            "InsertChecklistItem",
            checklistItemParam,
            commandType: CommandType.StoredProcedure,
            transaction: _unitOfWork.Transaction);
        }
      }
      return true;
    }
  }
}
