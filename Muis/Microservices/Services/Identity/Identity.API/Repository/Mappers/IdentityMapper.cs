using System;
using System.Collections.Generic;
using Core.Model;
using Identity.Model;

namespace Identity.API.Repository.Mappers
{
  public class IdentityMapper
  {
    readonly IDictionary<Guid, Model.Identity> _identityCache = new Dictionary<Guid, Model.Identity>();

    readonly IDictionary<long, Cluster> _clusterCache = new Dictionary<long, Cluster>();

    readonly IDictionary<long, Log> _logCache = new Dictionary<long, Log>();

    public Model.Identity Map(Model.Identity identity, RequestType? requestType = null,
      Cluster cluster = null, string node = null, Log log = null)
    {
      if (!_identityCache.TryGetValue(identity.ID, out Model.Identity result))
      {
        result = identity;
        _identityCache[identity.ID] = result;
      }

      if (   requestType != null
          && (!result.RequestTypes?.Contains(requestType.Value) ?? true))
      {
        if (result.RequestTypes == null)
        {
          result.RequestTypes = new List<RequestType>();
        }

        result.RequestTypes.Add(requestType.Value);
      }

      Cluster outCluster = null;
      if (  (cluster?.ID ?? 0) > 0
         && !_clusterCache.TryGetValue(cluster.ID, out outCluster))
      {
        _clusterCache[cluster.ID] = cluster;

        if (result.Clusters == null)
        {
          result.Clusters = new List<Cluster>();
        }

        result.Clusters.Add(cluster);
        outCluster = cluster;
      }

      if (!string.IsNullOrEmpty(node))
      {
        if (outCluster.Nodes == null)
        {
          outCluster.Nodes = new List<string>();
        }
        outCluster.Nodes.Add(node);
      }

      if (   (log?.ID ?? 0) > 0
          && !_logCache.ContainsKey(log.ID))
      {
        _logCache[log.ID] = log;

        if (result.Logs == null)
        {
          result.Logs = new List<Log>();
        }
        result.Logs.Add(log);
      }

      return result;
    }
  }
}