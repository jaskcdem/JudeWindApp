using JudeWindApp.Services;
using JudeWindApp.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using System.Text;

namespace JudeWindApp.Attributes
{
    /// <summary> WebAPI Log filter </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class AppLogFilterAttribute : Attribute, IAsyncActionFilter
    {
        /// <summary> 系統名稱 </summary>
        public string SystemName { get; set; }
        private readonly LogsService _logsService;
        //private readonly UserRecordService _userRecordService;

        //public AppLogFilterAttribute(LogsService logsService, UserRecordService userRecordService, string systemName)
        //{
        //    _logsService = logsService;
        //    _userRecordService = userRecordService;
        //    SystemName = systemName;
        //}
        /// <summary> WebAPI Log filter </summary>
        public AppLogFilterAttribute(LogsService logsService, string systemName)
        {
            _logsService = logsService;
            SystemName = systemName;
        }

        /// <summary> 當WebAPI的控制器剛被啟動和結束動作，會進入這個事件中 </summary>
        /// <param name="context"></param>
        /// <param name="next"></param>
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            ActionExecuting(context);
            await next();
            await ActionExecuted(context);
        }

        /// <summary>  當WebAPI的控制器剛被啟動的時候，會進入至這個覆寫的事件中 </summary>
        /// <param name="context"></param>
        private void ActionExecuting(ActionExecutingContext context)
        {
            DateTime dtStart = DateTime.Now;
            StringBuilder strInput = new();
            try
            {
                var controllerName = ((ControllerBase)context.Controller).ControllerContext.ActionDescriptor.ControllerName;
                var actionName = ((ControllerBase)context.Controller).ControllerContext.ActionDescriptor.ActionName;
                if (context.ActionArguments != null)
                {
                    // 因為傳入的參數為多數，所以ActionArguments必須用迴圈將之取出
                    foreach (var item in context.ActionArguments)
                    {
                        // 取出傳入的參數名稱
                        string strParamName = item.Key;

                        // 取出傳入的內容並作Json資料的處理
                        strInput.Append($"{strParamName}:{JsonConvert.SerializeObject(item.Value)}.");
                    }
                }
                // 將資料存入Context中
                context.HttpContext.Request.Headers.Add(new KeyValuePair<string, StringValues>("__StartTime__", new StringValues(dtStart.ToString())));
                context.HttpContext.Request.Headers.Add(new KeyValuePair<string, StringValues>("__InputData__", new StringValues(strInput.ToString())));
                context.HttpContext.Request.Headers.Add(new KeyValuePair<string, StringValues>("controllerName", controllerName));
                context.HttpContext.Request.Headers.Add(new KeyValuePair<string, StringValues>("actionName", actionName));
            }
            catch (Exception) { throw; }
        }

        /// <summary> 當WebAPI的控制器結束動作，會進入這個覆寫的事件中 </summary>
        /// <param name="context"></param>
        private async Task ActionExecuted(ActionExecutingContext context)
        {
            try
            {
                DateTime dtEnd = DateTime.Now;
                var strOutput = "";
                var jsonResult = context.Result as ObjectResult;
                strOutput = JsonConvert.SerializeObject(jsonResult?.Value);

                // 取得起始時間
                context.HttpContext.Request.Headers.TryGetValue("__StartTime__", out StringValues dtStartObj);
                // 取得Input資料
                context.HttpContext.Request.Headers.TryGetValue("__InputData__", out StringValues InputObj);
                // get log data
                var controllerName = context.HttpContext.Request.RouteValues.TryGetValue("controller", out var routeVal) ? routeVal?.ToString() ?? "wrongControler" : "failControler";
                var actionName = context.HttpContext.Request.RouteValues.TryGetValue("action", out routeVal) ? routeVal?.ToString() ?? "wrongAction" : "failAction";
                var host = context.HttpContext.Request.Headers.TryGetValue("Host", out StringValues headerValue) ? _logsService.GetSourceOrigin(headerValue.ToString()) : "failHost";
                var userId = context.HttpContext.Request.Headers.TryGetValue("clientid", out headerValue) ? headerValue.ToString() : "failClientid";
                var ipAddress = context.HttpContext.Connection.RemoteIpAddress?.MapToIPv4()?.ToString() ?? "?.?.?.1";
                if (context.HttpContext.Request.Headers.TryGetValue("X-Forwarded-For", out headerValue))
                    ipAddress = headerValue.ToString();

                var logTask = _logsService.WriteApiLog(new LogResultModel
                {
                    SystemName = SystemName,
                    ControllerName = controllerName,
                    ActionName = actionName,
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

                //var recordTask = _userRecordService.AddUserRecord(userId, actionName, InputObj.ToString(), ipAddress);
                //await Task.WhenAll(logTask, recordTask);
                await logTask;
            }
            catch (Exception) { }
        }
    }
}
