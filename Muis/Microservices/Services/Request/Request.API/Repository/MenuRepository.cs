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
  public class MenuRepository : IMenuRepository
  {
    readonly IUnitOfWork _unitOfWork;

    public MenuRepository(IUnitOfWork unitOfWork)
    {
      _unitOfWork = unitOfWork;
    }

    public async Task<IList<Menu>> Query(MenuFilter filter)
    {
      var param = new DynamicParameters();
      param.Add("@RequestID", filter.RequestID);
      param.Add("@Scheme", filter.Scheme);

      return (await SqlMapper.QueryAsync<Menu>(_unitOfWork.Connection,
        "QueryMenu",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction)).Distinct().ToList();
    }

    public async Task BulkUpdate(IList<Menu> menus, Guid userID, string userName)
    {
      if ((menus?.Count() ?? 0) == 0L)
      {
        return;
      }

      foreach (var menu in menus)
      {
        var param = new DynamicParameters();
        param.Add("@ID", menu.ID);
        param.Add("@Scheme", menu.Scheme);
        param.Add("@Text", menu.Text);
        param.Add("@SubText", menu.SubText);
        param.Add("@ChangeType", menu.ChangeType);

        await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
          "UpdateMenu",
          param,
          commandType: CommandType.StoredProcedure,
          transaction: _unitOfWork.Transaction);
      }
    }

    public async Task BulkAddReview(IList<Menu> menus, Guid userID, string userName)
    {
      if ((menus?.Count() ?? 0) == 0L)
      {
        return;
      }

      foreach (var menu in menus)
      {
        var param = new DynamicParameters();
        param.Add("@ID", menu.ID);
        param.Add("@Approved", menu.Approved);
        param.Add("@Remarks", menu.Remarks);
        param.Add("@ReviewedBy", userID);
        param.Add("@ReviewedByName", userName);

        await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
          "AddMenuReview",
          param,
          commandType: CommandType.StoredProcedure,
          transaction: _unitOfWork.Transaction);
      }
    }

    public async Task AddMenus(IList<Menu> menus, Guid userID, string userName)
    {
      if ((menus?.Count() ?? 0) == 0L)
      {
        return;
      }

      foreach (var menu in menus)
      {
        var param = new DynamicParameters();
        param.Add("Scheme", menu.Scheme);
        param.Add("Text", menu.Text);
        param.Add("SubText", menu.SubText);
        param.Add("ChangeType", menu.ChangeType);
        param.Add("RequestID", menu.RequestID);
        param.Add("Undeclared", menu.Undeclared);
        param.Add("@Approved", menu.Approved);
        param.Add("@Remarks", menu.Remarks);
        param.Add("@ReviewedBy", userID);
        param.Add("@ReviewedByName", userName);

        await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
          "AddMenus",
          param,
          commandType: CommandType.StoredProcedure,
          transaction: _unitOfWork.Transaction);
      }
    }
  }
}
