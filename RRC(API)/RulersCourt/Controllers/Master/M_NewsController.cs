using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using RulersCourt.Models;
using RulersCourt.Models.Master.M_News;
using RulersCourt.Repository.Master;
using System;
using System.Web;

namespace RulersCourt.Controllers.Master
{
    [Produces("application/json")]
    [Route("api/")]
    [ApiController]
    [Authorize]
    public class M_NewsController : CommonController
    {
        private readonly IOptions<ConnectionSettingsModel> appSettings;

        public M_NewsController(IServiceProvider serviceProvider, IOptions<ConnectionSettingsModel> app, IHostingEnvironment env)
            : base(app, env)
        {
            appSettings = app;
        }

        [HttpGet]
        [Route("News/{id:int}")]
        public IActionResult GetNews(int id)
        {
            int userID = int.Parse(Request.Query["UserID"]);
            var result = DbClientFactory<NewsClient>.Instance.GetNewsByID(appSettings.Value.ConnectionString, id, userID);
            return Ok(result);
        }

        [HttpPost]
        [Route("News")]
        public IActionResult PostNews([FromBody]M_NewsPostModel news)
        {
            var result = DbClientFactory<NewsClient>.Instance.PostNews(appSettings.Value.ConnectionString, news);
            NewsResponseModel res = new NewsResponseModel();
            res.NewsID = result.NewsID;
            return Ok(res);
        }

        [HttpPut]
        [Route("News")]
        public IActionResult PutNews([FromBody]M_NewsPutModel news)
        {
            var result = DbClientFactory<NewsClient>.Instance.PutNews(appSettings.Value.ConnectionString, news);
            NewsResponseModel res = new NewsResponseModel();
            res.NewsID = result.NewsID;
            return Ok(res);
        }

        [HttpDelete]
        [Route("News/{id:int}")]
        public IActionResult DeleteNews(int id)
        {
            var result = DbClientFactory<NewsClient>.Instance.DeleteNews(appSettings.Value.ConnectionString, id);
            return Ok(result);
        }

        [HttpGet]
        [Route("News/List/{PageNumber:int},{PageSize:int}")]
        public IActionResult GetNewsList(int pageNumber, int pageSize)
        {
            var parameters = HttpUtility.ParseQueryString(Request.QueryString.Value);
            var paramUserID = parameters["UserID"];
            var description = parameters["Description"];
            var response = DbClientFactory<NewsClient>.Instance.GetNewsList(appSettings.Value.ConnectionString, pageNumber, pageSize, paramUserID, description);
            return Ok(response);
        }

        [HttpGet]
        [Route("News/AllList")]
        public IActionResult GetNewsAllList()
        {
            var parameters = HttpUtility.ParseQueryString(Request.QueryString.Value);
            var paramUserID = parameters["UserID"];
            var response = DbClientFactory<NewsClient>.Instance.GetNewsAllList(appSettings.Value.ConnectionString, paramUserID);
            return Ok(response);
        }
    }
}
