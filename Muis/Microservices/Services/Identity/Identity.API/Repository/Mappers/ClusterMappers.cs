using Core.Model;
using Identity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.API.Repository.Mappers
{
  public class ClusterMappers
  {
    readonly Dictionary<long, Cluster> _cache = new Dictionary<long, Cluster>();

    readonly Dictionary<string, string> _nodesCache = new Dictionary<string, string>();

    readonly Dictionary<long, Log> _logCache = new Dictionary<long, Log>();

    readonly Dictionary<string, string> _allNodesCache = new Dictionary<string, string>();

    public Cluster Map(
      Cluster cluster,
      string nodes,
      Log log,
      string allNodes)
    {
      if (!_cache.TryGetValue(cluster.ID,
        out Cluster result))
      {
        cluster.Logs = new List<Log>();
        cluster.Nodes = new List<string>();
        cluster.AllNodes = "";
        _cache[cluster.ID] = cluster;
        result = cluster;
      }

      if (nodes != null
          && !_nodesCache.ContainsKey(nodes))
      {
        if (!result.Nodes.Contains(nodes))
        {
          result.Nodes.Add(nodes);
        }
      }

      if (allNodes != null
          && !_allNodesCache.ContainsKey(allNodes))
      {
        if (!result.AllNodes.Contains(allNodes))
        {
          result.AllNodes = allNodes;
        }
      }

      if (log.ID != 0
          && !_logCache.ContainsKey(log.ID))
      {
        result.Logs.Add(log);
        _logCache[log.ID] = log;
      }

      return result;

    }

    public Cluster Map(Cluster cluster, string nodes)
    {
      if (!_cache.TryGetValue(cluster.ID,
        out Cluster result))
      {
        cluster.Logs = new List<Log>();
        cluster.Nodes = new List<string>();
        cluster.AllNodes = "";
        _cache[cluster.ID] = cluster;
        result = cluster;
      }

      if (nodes != null
          && !_nodesCache.ContainsKey(nodes))
      {
        if (!result.Nodes.Contains(nodes))
        {
          result.Nodes.Add(nodes);
        }
      }  

      return result;

    }
  }
}
