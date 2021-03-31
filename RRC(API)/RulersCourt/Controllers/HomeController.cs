using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RulersCourt.Models;
using RulersCourt.Repository;
using System;
using System.Web;
using Workflow.Interface;

namespace RulersCourt.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class HomeController : ControllerBase
    {
        private readonly IOptions<ConnectionSettingsModel> appSettings;
        private readonly IOptions<UIConfigModel> uiSettings;
        private readonly IHostingEnvironment environment;
        private readonly IWorkflowClient workflow;
        private readonly IOptions<NotificationConfigModel> notify;

        public HomeController(IServiceProvider serviceProvider, IOptions<ConnectionSettingsModel> app, IHostingEnvironment env, IOptions<UIConfigModel> uiApp, IOptions<NotificationConfigModel> notification)
        {
            uiSettings = uiApp;
            appSettings = app;
            environment = env;
            workflow = serviceProvider.GetService<IWorkflowClient>();
            notify = notification;
        }

        [HttpGet]
        [Route("ModulesCount/")]
        public IActionResult GetAllTasksCount()
        {
            var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
            var parameters = HttpUtility.ParseQueryString(Request.QueryString.Value);
            var paramUserID = parameters["UserID"];
            var result = DbClientFactory<HomeClient>.Instance.GetModulesCount(appSettings.Value.ConnectionString, paramUserID, lang);
            return Ok(result);
        }

        [HttpGet]
        [Route("GlobalSearchList/{PageNumber:int},{PageSize:int}")]
        public IActionResult GetGlobalSearchList(int pageNumber, int pageSize)
        {
            try
            {
                var parameters = HttpUtility.ParseQueryString(Request.QueryString.Value);
                var paramUserID = parameters["UserID"];
                var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
                var paramSearch = parameters["SmartSearch"];
                var paramType = Convert.ToInt32(parameters["Type"]);
                var result = DbClientFactory<HomeClient>.Instance.GetGlobalSearchList(appSettings.Value.ConnectionString, paramUserID, paramSearch, lang, paramType, pageNumber, pageSize);
                return Ok(result);
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
        }

        [HttpGet]
        [Route("Notification/{PageNumber:int},{PageSize:int}")]
        public IActionResult GetNotificationList(int pageNumber, int pageSize)
        {
            try
            {
                var parameters = HttpUtility.ParseQueryString(Request.QueryString.Value);
                var paramUserID = parameters["UserID"];
                var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
                var result = DbClientFactory<HomeClient>.Instance.GetNotificationList(appSettings.Value.ConnectionString, paramUserID, pageNumber, pageSize, notify.Value, lang);
                return Ok(result);
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
        }

        [HttpPost]
        [Route("NotificationRead/{id}")]
        public IActionResult NotificationRead(int? id)
        {
            try
            {
                var parameters = HttpUtility.ParseQueryString(Request.QueryString.Value);
                int? paramUserID = Convert.ToInt32(parameters["UserID"]);
                bool? paramMarkAllAsRead = parameters["MarkAllAsRead"] == "true" ? true : false;
                var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
                var result = DbClientFactory<HomeClient>.Instance.ReadNotification(uiSettings.Value.Url, appSettings.Value.ConnectionString, paramUserID, id, paramMarkAllAsRead, lang);
                return Ok(result);
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
        }
    }
}
