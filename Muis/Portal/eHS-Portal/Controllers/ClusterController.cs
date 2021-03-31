using eHS.Portal.Infrastructure.Filters;
using eHS.Portal.Model;
using eHS.Portal.Models.Cluster;
using eHS.Portal.Services.Cluster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eHS.Portal.Controllers
{
  [Authorize]
  [ServiceFilter(typeof(SessionAwareFilter))]
  [PermissionFilter(Permission.SystemRead, Permission.SystemReadWrite)]
  public class ClusterController : Controller
  {
    readonly IClusterService _clusterService;

    public ClusterController(IClusterService clusterService)
    {
      _clusterService = clusterService;
    }

    [Route("api/[controller]/index")]
    public async Task<IList<Cluster>> IndexData()
    {
      return await _clusterService.Search();
    }

    [Route("[controller]")]
    public async Task<IActionResult> Cluster()
    {
      await Task.CompletedTask;
      return View(new ClusterModel
      {

      });
    }

    [Route("[controller]/createCluster")]
    public async Task<IActionResult> CreateCluster()
    {
      await Task.CompletedTask;

      return View(new ClusterModel
      {
        OClusters = await _clusterService.Search()
      });
    }

    [Route("[controller]/details/{id}")]
    public async Task<IActionResult> Details(long id)
    {
      var clusters = await _clusterService.Search();
      var otherClusters = (IList<Cluster>)clusters.Where(c => c.ID != id).ToList();

      return View(new ClusterModel
      {
        ID = id,
        Data = await _clusterService.GetByID(id),
        OClusters = otherClusters
      });
    }

    [HttpPost]
    [Route("api/[controller]/create")]
    public async Task<long> Post([FromBody] Cluster cluster)
    {
      return await _clusterService.AddCluster(cluster);
    }

    [HttpPut]
    [Route("api/[controller]")]
    public async Task<bool> Put([FromBody] Cluster cluster)
    {
      return await _clusterService.UpdateCluster(cluster);
    }

    [HttpDelete]
    [Route("api/[controller]/{id}")]
    public async Task<bool> Delete(long id)
    {
      await _clusterService.DeleteCluster(id);
      return true;
    }
  }
}
