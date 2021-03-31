using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using System.Data;
using Identity.API.Repository.Mappers;
using Core.API.Repository;
using Identity.API.Services;
using Identity.Model;
using Core.Model;

namespace Identity.API.Repository
{
  public class IdentityRepository : IIdentityRepository
  {
    readonly IUnitOfWork _unitOfWork;

    public IdentityRepository(IUnitOfWork unitOfWork)
    {
      _unitOfWork = unitOfWork;
    }

    public async Task<List<Model.Identity>> GetAllIdentityDetails(long id)
    {
      var param = new DynamicParameters();
      param.Add("@ID", id);

      var mapper = new IdentityMapper();

      return (await SqlMapper.QueryAsync(
        _unitOfWork.Connection,
        "GetAllIdentity",
        new[]
        {
        typeof(Model.Identity)
        },
        obj =>
        {
          var A = obj[0] as Model.Identity;
          return mapper.Map(A);
        },
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction)).ToList();
    }

    public async Task<IList<Model.Identity>> Query(IdentityFilter filter)
    {
      var requestTypes = new DataTable();
      requestTypes.Columns.Add("Val", typeof(int));

      if ((filter.RequestTypes?.Length ?? 0) > 0)
      {
        foreach (var requestType in filter.RequestTypes)
        {
          requestTypes.Rows.Add((int)requestType);
        }
      }

      var clusters = new DataTable();
      clusters.Columns.Add("Val", typeof(long));

      if ((filter.Clusters?.Length ?? 0) > 0)
      {
        foreach (var cluster in filter.Clusters)
        {
          clusters.Rows.Add(cluster);
        }
      }

      var permissions = new DataTable();
      permissions.Columns.Add("Val", typeof(int));

      if ((filter.Permissions?.Length ?? 0) > 0)
      {
        foreach (var permission in filter.Permissions)
        {
          permissions.Rows.Add((int)permission);
        }
      }

      var ids = new DataTable();
      ids.Columns.Add("Val", typeof(Guid));

      if ((filter.IDs?.Length ?? 0) > 0)
      {
        foreach (var id in filter.IDs)
        {
          ids.Rows.Add(id);
        }
      }

      var nodes = new DataTable();
      nodes.Columns.Add("Val", typeof(string));

      if ((filter.Nodes?.Length ?? 0) > 0)
      {
        foreach (var node in filter.Nodes)
        {
          nodes.Rows.Add(node);
        }
      }

      var param = new DynamicParameters();
      param.Add("@Name", filter.Name);
      param.Add("@Email", filter.Email);
      param.Add("@Status", filter.Status);
      param.Add("Permissions", permissions.AsTableValuedParameter("dbo.SmallIntType"));
      param.Add("IDs", ids.AsTableValuedParameter("dbo.UniqueIdentifierType"));
      param.Add("RequestTypes", requestTypes.AsTableValuedParameter("dbo.SmallIntType"));
      param.Add("Clusters", clusters.AsTableValuedParameter("dbo.BigIntType"));
      param.Add("Nodes", nodes.AsTableValuedParameter("dbo.NvarcharType"));

      var mapper = new IdentityMapper();

      return (await SqlMapper.QueryAsync(_unitOfWork.Connection,
        "SelectIdentity",
        new[]
        {
          typeof(Model.Identity),
          typeof(RequestType?),
          typeof(Cluster),
          typeof(string)
        },
        obj =>
        {
          var identity = obj[0] as Model.Identity;
          var requestType = obj[1] as RequestType?;
          var cluster = obj[2] as Cluster;
          var node = obj[3] as string;

          return mapper.Map(identity, requestType, cluster, node);
        },
        param,
        splitOn: "ID,RequestType,ClusterID,Node",
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction)).Distinct().ToList();
    }

    public async Task Insert(Model.Identity identity)
    {
      var param = new DynamicParameters();
      param.Add("@ID", identity.ID);
      param.Add("@Name", identity.Name);
      param.Add("@Designation", identity.Designation);
      param.Add("@Role", identity.Role);
      param.Add("@Permissions", identity.Permissions);
      param.Add("@Email", identity.Email.ToLower());
      param.Add("@Status", identity.Status);

      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        "InsertIdentity",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);
    }

    public async Task Update(Model.Identity identity)
    {
      var param = new DynamicParameters();
      param.Add("@ID", identity.ID);
      param.Add("@Name", identity.Name);
      param.Add("@Designation", identity.Designation);
      param.Add("@Role", identity.Role);
      param.Add("@Permissions", identity.Permissions);
      param.Add("@Email", identity.Email.ToLower());
      param.Add("@Status", identity.Status);

      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        "UpdateIdentity",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);
    }

    public async Task MapIdentityToClusters(Guid id, IList<Cluster> clusters)
    {
      var table = new DataTable();
      table.Columns.Add("Val", typeof(long));

      if (clusters?.Any() ?? false)
      {
        foreach(var cluster in clusters)
        {
          table.Rows.Add(cluster.ID);
        }
      }

      var param = new DynamicParameters();
      param.Add("@ID", id);
      param.Add("@Clusters", table.AsTableValuedParameter("dbo.BigIntType"));

      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        "MapIdentityToClusters",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);
    }

    public async Task MapIdentityToRequestTypes(Guid id, IList<RequestType> requestTypes)
    {
      var table = new DataTable();
      table.Columns.Add("Val", typeof(int));

      if (requestTypes?.Any() ?? false)
      {
        foreach (var requestType in requestTypes)
        {
          table.Rows.Add((int)requestType);
        }
      }

      var param = new DynamicParameters();
      param.Add("@ID", id);
      param.Add("@RequestTypes", table.AsTableValuedParameter("dbo.SmallIntType"));

      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        "MapIdentityToRequestTypes",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);
    }

    public async Task<Model.Identity> GetIdentityByID(Guid id)
    {
      var param = new DynamicParameters();
      param.Add("@ID", id);

      var mapper = new IdentityMapper();

      return (await SqlMapper.QueryAsync(_unitOfWork.Connection,
        "GetIdentityByID",
        new[]
        {
          typeof(Model.Identity),
          typeof(RequestType?),
          typeof(Cluster),
          typeof(string),
          typeof(Log)
        },
        obj =>
        {
          var identity = obj[0] as Model.Identity;
          var requestType = obj[1] as RequestType?;
          var cluster = obj[2] as Cluster;
          var node = obj[3] as string;
          var log = obj[4] as Log;

          return mapper.Map(identity, requestType, cluster, node, log);
        },
        param,
        splitOn: "ID,RequestType,ClusterID,Node,LogID",
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction)).FirstOrDefault();
    }

    public async Task MapIdentityToLog(Guid id, long logID)
    {
      var param = new DynamicParameters();
      param.Add("@IdentityID", id);
      param.Add("@LogID", logID);

      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        "MapIdentityToLog",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);
    }

    public async Task UpdateIdentitySequence(Model.Identity identity)
    {
      var param = new DynamicParameters();
      param.Add("@ID", identity.ID);
      param.Add("@Sequence", identity.Sequence);     

      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        "UpdateIdentitySequence",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);
    }
  }
}
