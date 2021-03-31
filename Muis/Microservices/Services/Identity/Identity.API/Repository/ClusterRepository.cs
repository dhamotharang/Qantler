using Core.API.Repository;
using Core.Model;
using Dapper;
using Identity.API.Repository.Converters;
using Identity.API.Repository.Mappers;
using Identity.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.API.Repository
{
  public class ClusterRepository : IClusterRepository
  {
    readonly IUnitOfWork _unitOfWork;

    public ClusterRepository(IUnitOfWork unitOfWork)
    {
      _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<Cluster>> Select()
    {
      var mapper = new ClusterMappers();

      return (await SqlMapper.QueryAsync(
        _unitOfWork.Connection,
        "GetCluster",
        new[]
        {
        typeof(Cluster),
        typeof(string),
        typeof(Log),
        typeof(string)
        },
        obj =>
        {
          return mapper.Map(obj[0] as Cluster,
            obj[1] as string,
            obj[2] as Log,
            obj[3] as string);
        },
        splitOn: "ID,Nodes,LogID,AllNodes",
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction)).Distinct();
    }

    public async Task<Cluster> GetClusterByID(long id)
    {
      var clusterMapper = new ClusterMappers();

      var param = new DynamicParameters();
      param.Add("@ID", id);

      return (await SqlMapper.QueryAsync(
        _unitOfWork.Connection,
        "GetClusterByID",
        new[]
        {
          typeof(Cluster),
          typeof(string),
          typeof(Log),
          typeof(string)
        },
        obj =>
        {
          return clusterMapper.Map(obj[0] as Cluster,
            obj[1] as string,
            obj[2] as Log,
            obj[3] as string);
        },
        param,
        splitOn: "ID,Nodes,LogID,AllNodes",
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction)).FirstOrDefault();
    }

    public async Task<long> AddCluster(Cluster cluster)
    {
      DataTable dr = new DataTable();

      dr.Columns.Add("Node", typeof(string));

      if (cluster.Nodes != null)
      {
        foreach (string item in cluster.Nodes)
        {
          dr.Rows.Add(item);
        }
      }

      var param = new DynamicParameters();
      param.Add("@district", cluster.District);
      param.Add("@locations", cluster.Locations);
      param.Add("@color", cluster.Color);
      param.Add("@cNode", dr.AsTableValuedParameter("dbo.NodesType"));     
      param.Add("@outID", dbType: DbType.Int64, direction: ParameterDirection.Output);

      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        "InsertCluster",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);

      return param.Get<long>("@outID"); ;
    }

    public async Task<bool> UpdateCluster(Cluster cluster)
    {
      DataTable dr = new DataTable();

      dr.Columns.Add("Node", typeof(string));

      if (cluster.Nodes != null)
      {
        foreach (string item in cluster.Nodes)
        {
          dr.Rows.Add(item);
        }
      }

      var param = new DynamicParameters();
      param.Add("@cID", cluster.ID);
      param.Add("@district", cluster.District);
      param.Add("@locations", cluster.Locations);
      param.Add("@cNode", dr.AsTableValuedParameter("dbo.NodesType"));

      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        "UpdateCluster",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);

      return true;
    }

    public async Task<bool> DeleteCluster(long id)
    {
      var param = new DynamicParameters();
      param.Add("@id", id);

      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        "DeleteCluster",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);

      return true;
    }

    public async Task<IEnumerable<Cluster>> GetClusterByNode(string cNode)
    {
      var mapper = new ClusterMappers();

      var param = new DynamicParameters();
      param.Add("@Node", cNode);

      return (await SqlMapper.QueryAsync(
        _unitOfWork.Connection,
        "GetClusterByNode",
        new[]
        {
        typeof(Cluster),
        typeof(string)
        },
        obj =>
        {
          return mapper.Map(obj[0] as Cluster, obj[1] as string);
        },
        param,
        splitOn: "ID,Nodes",
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction));
    }

    public async Task MapLog(long clusterID, long logID)
    {
      var param = new DynamicParameters();
      param.Add("@ClusterID", clusterID);
      param.Add("@LogID", logID);

      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        "MapClusterToLog",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);
    }
  }
}
