using JudeWind.Model.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace JudeWindApp.Attributes
{
    /// <summary> WebAPI result filter </summary>
    public class AppResultFilter : IResultFilter
    {
        /// <summary>  </summary>
        public void OnResultExecuted(ResultExecutedContext context)
        {
            // do nothing
        }

        /// <summary>  </summary>
        public void OnResultExecuting(ResultExecutingContext context)
        {
            if (context.ActionDescriptor.FilterDescriptors.Any(x => x.Filter is BypassApiResultAttribute))
                return;

            //run code immediately before and after the execution of action results. They run only when the action method has executed successfully. They are useful for logic that must surround view or formatter execution.
            if (context.Result is not ObjectResult result) return;
            //change this ResultApi with your ApiResponse class
            BaseResult<object> resp;
            if (result.StatusCode == 400)
            {
                resp = new BaseResult<object>
                {
                    // API 返回的狀態碼
                    Code = StatusCodes.Status400BadRequest,
                    // 取得由 API 返回的資料
                    Body = result.Value ?? string.Empty
                };
            }
            else
            {
                resp = new BaseResult<object>
                {
                    // API 返回的狀態碼
                    Code = StatusCodes.Status200OK,
                    // 取得由 API 返回的資料
                    Body = result?.Value ?? string.Empty
                };
            }

            context.Result = new JsonResult(resp, new JsonSerializerOptions()
            {
                Converters = { new JsonStringEnumConverter() }
            });
        }
    }
}
