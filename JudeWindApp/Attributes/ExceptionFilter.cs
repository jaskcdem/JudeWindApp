using JudeWind.Model.Base;
using JudeWindApp.Services;
using JudeWindApp.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace JudeWindApp.Attributes
{
    /// <summary>Exception Handler</summary>
    public class ExceptionFilter(LogsService logService) : IAsyncExceptionFilter
    {
        /// <summary>Catch exception and log</summary>
        /// <exception cref="NotImplementedException"></exception>
        public Task OnExceptionAsync(ExceptionContext context)
        {
            if (context != null && !context.ExceptionHandled)
            {
                var _exception = new BaseExceptionMessageModel();
                Exception ex = context.ExceptionDispatchInfo != null ? context.ExceptionDispatchInfo.SourceException : context.Exception;
                _exception.Message = ex.Message ?? string.Empty;
                _exception.StrackTrace = ex.StackTrace ?? string.Empty;
                var id = logService.WriteException(new LoggerModel
                {
                    Code = "ExceptionFilter",
                    Level = NLogLevel.Error
                }, ex).GetAwaiter().GetResult();
                _exception.OtherMessage = $"Id: {id}";

                var result = new BaseResult<BaseExceptionMessageModel>
                {
                    Code = (int)HttpStatusCode.InternalServerError,
                    Body = _exception,
                    Exception = ex,
                };
                context.Result = new JsonResult(result);
                context.ExceptionHandled = true;
            }
            return Task.CompletedTask;
        }
    }
}
