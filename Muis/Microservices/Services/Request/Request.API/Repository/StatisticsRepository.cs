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
  public class StatisticsRepository : IStatisticsRepository
  {
    readonly IUnitOfWork _uow;

    public StatisticsRepository(IUnitOfWork uow)
    {
      _uow = uow;
    }

    public async Task<IList<StatisticsPerformance>> Performance(IList<string> keys,
      DateTimeOffset? from = null,
      DateTimeOffset? to = null)
    {
      var keysTable = new DataTable();
      keysTable.Columns.Add("Val", typeof(string));

      if (keys?.Any() ?? false)
      {
        foreach(var val in keys)
        {
          keysTable.Rows.Add(val);
        }
      }

      var param = new DynamicParameters();
      param.Add("@Keys", keysTable.AsTableValuedParameter("dbo.VarcharType"));
      param.Add("@From", from);
      param.Add("@To", to);

      return (await SqlMapper.QueryAsync<StatisticsPerformance>(_uow.Connection,
        "StatsPerformance",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _uow.Transaction)).ToList();
    }

    public async Task<IList<StatisticsOverview>> Overview(DateTimeOffset? from = null,
      DateTimeOffset? to = null)
    {
      var param = new DynamicParameters();
      param.Add("@From", from);
      param.Add("@To", to);

      return (await SqlMapper.QueryAsync<StatisticsOverview>(_uow.Connection,
        "StatsOverview",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _uow.Transaction)).ToList();
    }

    public async Task<IList<StatisticsStatus>> Status(DateTimeOffset? from = null,
      DateTimeOffset? to = null)
    {
      var param = new DynamicParameters();
      param.Add("@From", from);
      param.Add("@To", to);

      return (await SqlMapper.QueryAsync<StatisticsStatus>(_uow.Connection,
        "StatsStatus",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _uow.Transaction)).ToList();
    }
  }
}
