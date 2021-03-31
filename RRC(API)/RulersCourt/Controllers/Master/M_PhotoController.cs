using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using RulersCourt.Models;
using RulersCourt.Models.Master.M_Photos;
using RulersCourt.Repository.Master;
using System;
using System.Web;

namespace RulersCourt.Controllers.Master
{
    [Produces("application/json")]
    [Route("api/")]
    [ApiController]
    [Authorize]
    public class M_PhotoController : CommonController
    {
        private readonly IOptions<ConnectionSettingsModel> appSettings;

        public M_PhotoController(IServiceProvider serviceProvider, IOptions<ConnectionSettingsModel> app, IHostingEnvironment env)
            : base(app, env)
        {
            appSettings = app;
        }

        [HttpGet]
        [Route("Photos/{id:int}")]
        public IActionResult GetPhoto(int id)
        {
            M_PhotoGetModel response = new M_PhotoGetModel();
            int userID = int.Parse(Request.Query["UserID"]);
            var result = DbClientFactory<PhotosClient>.Instance.GetPhotosByID(appSettings.Value.ConnectionString, id, userID);
            return Ok(result);
        }

        [HttpPost]
        [Route("Photos")]
        public IActionResult PostPhoto([FromBody]M_PhotoPostModel photo)
        {
            var result = DbClientFactory<PhotosClient>.Instance.PostPhoto(appSettings.Value.ConnectionString, photo);
            PhotoResponseModel res = new PhotoResponseModel();
            res.PhotoID = result.PhotoID;
            return Ok(res);
        }

        [HttpPut]
        [Route("Photos")]
        public IActionResult PutPhoto([FromBody]M_PhotoPutModel photo)
        {
            var result = DbClientFactory<PhotosClient>.Instance.PutPhoto(appSettings.Value.ConnectionString, photo);
            PhotoResponseModel res = new PhotoResponseModel();
            res.PhotoID = result.PhotoID;
            return Ok(res);
        }

        [HttpDelete]
        [Route("Photos/{id:int}")]
        public IActionResult DeletePhoto(int id)
        {
            var result = DbClientFactory<PhotosClient>.Instance.DeletePhoto(appSettings.Value.ConnectionString, id);
            return Ok(result);
        }

        [HttpGet]
        [Route("Photos/List/{PageNumber:int},{PageSize:int}")]
        public IActionResult GetPhotoList(int pageNumber, int pageSize)
        {
            var parameters = HttpUtility.ParseQueryString(Request.QueryString.Value);
            var paramUserID = parameters["UserID"];

            var description = parameters["Description"];
            var response = DbClientFactory<PhotosClient>.Instance.GetPhotoList(appSettings.Value.ConnectionString, pageNumber, pageSize, paramUserID, description);
            return Ok(response);
        }

        [HttpGet]
        [Route("Photos/AllList")]
        public IActionResult GetPhotoAllList()
        {
            var parameters = HttpUtility.ParseQueryString(Request.QueryString.Value);
            var paramUserID = parameters["UserID"];
            var response = DbClientFactory<PhotosClient>.Instance.GetPhotoAllList(appSettings.Value.ConnectionString, paramUserID);
            return Ok(response);
        }
    }
}