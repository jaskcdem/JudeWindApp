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
            BaseResult<object> resp = result.StatusCode == 400
                ? new BaseResult<object>
                {
                    Code = StatusCodes.Status400BadRequest,
                    Body = result.Value ?? string.Empty
                }
                : new BaseResult<object>
                {
                    Code = StatusCodes.Status200OK,
                    Body = result?.Value ?? string.Empty
                };
            context.Result = new JsonResult(resp, new JsonSerializerOptions()
            {
                Converters = { new JsonStringEnumConverter() }
            });
        }
    }
}
