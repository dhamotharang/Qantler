using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using RulersCourt.Models;
using RulersCourt.Models.Master.M_Photos;
using RulersCourt.Repository.Master;
using System;

namespace RulersCourt.Controllers.Master
{
    [Produces("application/json")]
    [Route("api/")]
    [ApiController]
    [Authorize]
    public class M_BannerController : CommonController
    {
        private readonly IOptions<ConnectionSettingsModel> appSettings;

        public M_BannerController(IServiceProvider serviceProvider, IOptions<ConnectionSettingsModel> app, IHostingEnvironment env)
            : base(app, env)
        {
            appSettings = app;
        }

        [HttpGet]
        [Route("Banner")]
        public IActionResult GetPhoto()
        {
            M_PhotoGetModel response = new M_PhotoGetModel();
            var result = DbClientFactory<M_BannerClient>.Instance.GetBanner(appSettings.Value.ConnectionString);
            return Ok(result);
        }

        [HttpPost]
        [Route("Banner")]
        public IActionResult PostPhoto([FromBody]M_BannerPostModel banner)
        {
            var result = DbClientFactory<M_BannerClient>.Instance.PostBanner(appSettings.Value.ConnectionString, banner);
            return Ok(result);
        }
    }
}