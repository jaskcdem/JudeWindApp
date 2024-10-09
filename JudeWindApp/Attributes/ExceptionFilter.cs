using JudeWind.Model.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace JudeWindApp.Attributes
{
    /// <summary> 例外處理AOP </summary>
    public class ExceptionFilter : IAsyncExceptionFilter
    {
        /// <summary> 攔截Exception事件 </summary>
        /// <exception cref="NotImplementedException"></exception>
        public Task OnExceptionAsync(ExceptionContext context)
        {
            // 確認仍未處理
            if (context != null && !context.ExceptionHandled)
            {
                var _exception = new BaseExceptionMessageModel();
                if (context.ExceptionDispatchInfo != null)
                {
                    _exception.Message = context.ExceptionDispatchInfo.SourceException.Message ?? string.Empty;
                    _exception.StrackTrace = context.ExceptionDispatchInfo.SourceException.StackTrace ?? string.Empty;
                }
                else
                {
                    _exception.Message = context.Exception.Message ?? string.Empty;
                    _exception.StrackTrace = context.Exception.StackTrace ?? string.Empty;
                }

                var result = new BaseResult<BaseExceptionMessageModel>
                {
                    Code = (int)HttpStatusCode.InternalServerError,
                    Body = _exception,
                    Exception = context.Exception
                };
                context.Result = new JsonResult(result);
                context.ExceptionHandled = true;
            }
            return Task.CompletedTask;
        }
    }
}
