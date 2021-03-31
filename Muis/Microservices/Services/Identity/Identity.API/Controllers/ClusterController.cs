using Identity.API.Services;
using Identity.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Identity.API.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class ClusterController : ControllerBase
  {
    readonly IClusterService _clusterService;

    public ClusterController(IClusterService clusterService)
    {
      _clusterService = clusterService;
    }

    [HttpGet]
    [Route("query")]
    public async Task<IEnumerable<Cluster>> Query()
    {
      return await _clusterService.QueryCluster();
    }

    [HttpGet]
    [Route("{id:int}")]
    public async Task<Cluster> Get(
      long id)
    {
      return await _clusterService.GetClusterByID(id);
    }

    [HttpPost]
    public async Task<long> Post([FromBody] Cluster cluster, Guid userID, string userName)
    {
      return await _clusterService.AddCluster(cluster, userID, userName);
    }

    [HttpPut]
    public async Task<bool> Put([FromBody] Cluster cluster, Guid userID, string userName)
    {
        return await _clusterService.UpdateCluster(cluster, userID, userName); 
    }

    [HttpDelete]
    [Route("{id:int}")]
    public async Task<bool> DeleteCluster(long id, Guid userID, string userName)
    {
      await _clusterService.DeleteCluster(id, userID, userName);
      return true;
    }
  }
}
