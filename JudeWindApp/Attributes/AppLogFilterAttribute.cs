using JudeWind.Service.Register;
using JudeWindApp.Services;
using JudeWindApp.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using System.Text;

namespace JudeWindApp.Attributes
{
    /// <summary>WebAPI Log filter</summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class AppLogFilterAttribute(LogsService logsService, UserRecordService userRecordService, string systemName) : Attribute, IAsyncActionFilter
    {
        /// <summary></summary>
        public string SystemName { get; set; } = systemName;

        /// <summary>Excute when API start or end,</summary>
        /// <param name="context"></param>
        /// <param name="next"></param>
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            ActionExecuting(context);
            await next();
            await ActionExecuted(context);
        }

        /// <summary>Excute when API start,</summary>
        /// <param name="context"></param>
        private static void ActionExecuting(ActionExecutingContext context)
        {
            DateTime dtStart = DateTime.Now;
            StringBuilder strInput = new();
            try
            {
                var controllerName = ((ControllerBase)context.Controller).ControllerContext.ActionDescriptor.ControllerName;
                var actionName = ((ControllerBase)context.Controller).ControllerContext.ActionDescriptor.ActionName;
                if (context.ActionArguments != null)
                    strInput.Append(JsonConvert.SerializeObject(context.ActionArguments));
                //Save into Request Headers
                context.HttpContext.Request.Headers.Add(new KeyValuePair<string, StringValues>("__StartTime__", new StringValues(dtStart.ToString())));
                context.HttpContext.Request.Headers.Add(new KeyValuePair<string, StringValues>("__InputData__", new StringValues(strInput.ToString())));
                context.HttpContext.Request.Headers.Add(new KeyValuePair<string, StringValues>("controllerName", controllerName));
                context.HttpContext.Request.Headers.Add(new KeyValuePair<string, StringValues>("actionName", actionName));
            }
            catch (Exception) { throw; }
        }

        /// <summary>Excute when API end,</summary>
        /// <param name="context"></param>
        private async Task ActionExecuted(ActionExecutingContext context)
        {
            try
            {
                DateTime dtEnd = DateTime.Now;
                var strOutput = "";
                var jsonResult = context.Result as ObjectResult;
                strOutput = JsonConvert.SerializeObject(jsonResult?.Value);

                context.HttpContext.Request.Headers.TryGetValue("__StartTime__", out StringValues dtStartObj);
                context.HttpContext.Request.Headers.TryGetValue("__InputData__", out StringValues InputObj);
                // get log data
                var controllerName = context.HttpContext.Request.RouteValues.TryGetValue("controller", out var routeVal) ? routeVal?.ToString() ?? "wrongControler" : "failControler";
                var actionName = context.HttpContext.Request.RouteValues.TryGetValue("action", out routeVal) ? routeVal?.ToString() ?? "wrongAction" : "failAction";
                var host = context.HttpContext.Request.Headers.TryGetValue("Host", out StringValues headerValue) ? logsService.GetSourceOrigin(headerValue.ToString()) : "failHost";
                var userId = context.HttpContext.Request.Headers.TryGetValue("clientid", out headerValue) ? headerValue.ToString() : "failClientid";
                var ipAddress = context.HttpContext.Connection.RemoteIpAddress?.MapToIPv4()?.ToString() ?? "?.?.?.1";
                if (context.HttpContext.Request.Headers.TryGetValue("X-Forwarded-For", out headerValue))
                    ipAddress = headerValue.ToString();

                var logTask = logsService.WriteApiLog(new LoggerModel
                {
                    Code = $"{controllerName}.{actionName}",
                    Level = NLogLevel.Info
                }, new LogResultModel
                {
                    SystemName = SystemName,
                    ControllerName = controllerName,
                    ActionName = actionName,
                    IpAddress = ipAddress,
                    Source = host,
                    Header = context.HttpContext.Request.Headers,
                    StartTime = Convert.ToDateTime(dtStartObj.ToString()),
                    EndTime = dtEnd,
                    Input = InputObj.ToString(),
                    Output = strOutput,
                    UserId = userId,
                    Exception = "",
                    SessionID = context.HttpContext?.Session?.Id ?? string.Empty
                });

                var recordTask = userRecordService.AddUserRecord(userId, actionName, InputObj.ToString(), ipAddress);
                await Task.WhenAll(logTask, recordTask);
            }
            catch (Exception ex)
            {
                await logsService.WriteException(new LoggerModel
                {
                    Code = "AppLogFilterAttribute",
                    Level = NLogLevel.Warn
                }, ex);
            }
        }
    }
}
