using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspCoreServer.Controllers
{

	[Authorize]
	[Produces("application/json")]
    [Route("api/Upload")]
    public class UploadController : Controller
    {
    
	 }
}
