using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Core.API.Repository;
using Dapper;
using Request.Model;

namespace Request.API.Repository
{
  public class IngredientRepository : IIngredientRepository
  {
    readonly IUnitOfWork _unitOfWork;

    public IngredientRepository(IUnitOfWork unitOfWork)
    {
      _unitOfWork = unitOfWork;
    }

    public async Task<IList<Ingredient>> Query(IngredientFilter filter)
    {
      var typeTable = new DataTable();
      typeTable.Columns.Add("Val", typeof(int));

      if (filter.RiskCategory?.Any() ?? false)
      {
        foreach (var type in filter.RiskCategory)
        {
          typeTable.Rows.Add(type);
        }
      }
      var param = new DynamicParameters();
      param.Add("@RequestID", filter.RequestID);
      param.Add("@RiskCategory", typeTable.AsTableValuedParameter("dbo.SmallIntType"));

      return (await SqlMapper.QueryAsync<Ingredient>(_unitOfWork.Connection,
        "QueryIngredient",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction)).Distinct().ToList();
    }

    public async Task BulkUpdate(IList<Ingredient> ingredients, Guid userID, string userName)
    {
      foreach (var ingredient in ingredients)
      {
        var param = new DynamicParameters();
        param.Add("@ID", ingredient.ID);
        param.Add("@Text", ingredient.Text);
        param.Add("@SubText", ingredient.SubText);
        param.Add("@RiskCategory", ingredient.RiskCategory);
        param.Add("@ChangeType", ingredient.ChangeType);

        await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
          "UpdateIngredient",
          param,
          commandType: CommandType.StoredProcedure,
          transaction: _unitOfWork.Transaction);
      }
    }

    public async Task BulkAddReview(IList<Ingredient> ingredients, Guid userID, string userName)
    {
      if ((ingredients?.Count() ?? 0) == 0L)
      {
        return;
      }

      foreach (var ingredient in ingredients)
      {
        var param = new DynamicParameters();
        param.Add("@ID", ingredient.ID);
        param.Add("@Approved", ingredient.Approved);
        param.Add("@Remarks", ingredient.Remarks);
        param.Add("@ReviewedBy", userID);
        param.Add("@ReviewedByName", userName);

        await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
          "AddIngredientReview",
          param,
          commandType: CommandType.StoredProcedure,
          transaction: _unitOfWork.Transaction);
      }
    }

    public async Task AddIngredients(IList<Ingredient> ingredients, Guid userID, string userName)
    {
      if ((ingredients?.Count() ?? 0) == 0L)
      {
        return;
      }

      foreach (var ingredient in ingredients)
      {
        var param = new DynamicParameters();
        param.Add("@Text", ingredient.Text);
        param.Add("@SubText", ingredient.SubText);
        param.Add("@RiskCategory", ingredient.RiskCategory);
        param.Add("@ChangeType", ingredient.ChangeType);
        param.Add("@RequestID", ingredient.RequestID);
        param.Add("@Undeclared", ingredient.Undeclared);
        param.Add("@Approved", ingredient.Approved);
        param.Add("@Remarks", ingredient.Remarks);
        param.Add("@ReviewedBy", userID);
        param.Add("@ReviewedByName", userName);
        
        await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
          "AddIngredients",
          param,
          commandType: CommandType.StoredProcedure,
          transaction: _unitOfWork.Transaction);
      }
    }
  }
}
