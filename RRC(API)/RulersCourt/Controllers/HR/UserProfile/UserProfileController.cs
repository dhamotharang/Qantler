using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RulersCourt.Models;
using RulersCourt.Models.UserProfile;
using RulersCourt.Repository.UserProfile;
using System;
using System.Web;
using Workflow.Interface;

namespace RulersCourt.Controllers.UserProfile
{
    [EnableCors("AllowOrigin")]
    [Produces("application/json")]
    [Authorize]
    [Route("api/")]
    public class UserProfileController : CommonController
    {
        private readonly IOptions<ConnectionSettingsModel> appSettings;
        private readonly IWorkflowClient workflow;
        private readonly IHostingEnvironment environment;

        public UserProfileController(IOptions<ConnectionSettingsModel> app, IServiceProvider serviceProvider, IHostingEnvironment env)
            : base(app, env)
        {
            appSettings = app;
            environment = env;
            workflow = serviceProvider.GetService<IWorkflowClient>();
        }

        [HttpGet]
        [ServiceFilter(typeof(AccessControlAttribute))]
        [Route("UserProfile/{id}")]
        public IActionResult GetUserProfileByID(int id)
        {
            var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
            int userID = int.Parse(Request.Query["UserID"]);
            var result = DbClientFactory<UserProfileClient>.Instance.GetUserProfileByID(appSettings.Value.ConnectionString, id, userID, lang);
            return Ok(result);
        }

        [HttpPost]
        [ServiceFilter(typeof(AccessControlAttribute))]
        [Route("UserProfile")]
        public IActionResult SaveUserProfile([FromBody]UserProfilePostModel userProfile)
        {
            var result = DbClientFactory<UserProfileClient>.Instance.PostUserProfile(appSettings.Value.ConnectionString, userProfile);
            UserProfileSaveResponseModel res = new UserProfileSaveResponseModel();
            res.UserProfileId = result.UserProfileId;
            res.EmployeeCode = result.EmployeeCode;
            return Ok(res);
        }

        [HttpPut]
        [ServiceFilter(typeof(AccessControlAttribute))]
        [Route("UserProfile")]
        public IActionResult UpdateUserProfile([FromBody]UserProfilePutModel userProfile)
        {
            int userID = int.Parse(Request.Query["UserID"]);
            var result = DbClientFactory<UserProfileClient>.Instance.PutUserProfile(appSettings.Value.ConnectionString, userProfile);
            UserProfileSaveResponseModel res = new UserProfileSaveResponseModel();
            res.UserProfileId = result.UserProfileId;
            res.EmployeeCode = result.EmployeeCode;
            return Ok(res);
        }

        [HttpDelete]
        [ServiceFilter(typeof(AccessControlAttribute))]
        [Route("UserProfile/{id:int}")]
        public IActionResult DeleteUserProfile(int id)
        {
            int userID = int.Parse(Request.Query["UserID"]);
            var result = DbClientFactory<UserProfileClient>.Instance.DeleteUserProfile(appSettings.Value.ConnectionString, id);
            return Ok(result);
        }

        [HttpPatch]
        [Route("UserProfile/{id}")]
        public IActionResult ModifyUserProfile(int id, [FromBody]JsonPatchDocument<UserProfilePutModel> value)
        {
            var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
            int userID = int.Parse(Request.Query["UserID"]);
            var result = DbClientFactory<UserProfileClient>.Instance.PatchUserProfile(appSettings.Value.ConnectionString, id, value, lang);
            UserProfileSaveResponseModel res = new UserProfileSaveResponseModel();
            res.UserProfileId = result.UserProfileId;
            res.EmployeeCode = result.EmployeeCode;
            return Ok(res);
        }

        [HttpGet]
        [Route("UserProfile/Home/List/{PageNumber:int},{PageSize:int}")]
        public IActionResult GetAllUserProfiles(int pageNumber, int pageSize)
        {
            var parameters = HttpUtility.ParseQueryString(Request.QueryString.Value);
            var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
            var paramUserName = parameters["UserName"];
            var paramOrgUnitID = parameters["OrgUnitID"];
            var paramJobTitle = parameters["JobTitle"];
            var paramSmartSearch = parameters["SmartSearch"];
            var paramType = parameters["Type"];
            var response = DbClientFactory<UserProfileClient>.Instance.GetUserProfile(appSettings.Value.ConnectionString, pageNumber, pageSize, paramUserName, paramOrgUnitID, paramSmartSearch, paramJobTitle, paramType, lang);
            return Ok(response);
        }

        [HttpGet]
        [Route("UserProfile/Home/Count/{UserID:int}")]
        public IActionResult GetHomeCount(int userID)
        {
            var response = DbClientFactory<UserProfileClient>.Instance.GetUserProfileHomeCount(appSettings.Value.ConnectionString, userID);
            return Ok(response);
        }
    }
}
