using JudeWind.Model.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace JudeWindApp.Attributes
{
    /// <summary> Validate Model </summary>
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        /// <summary>  </summary>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var errors = context.ModelState.Where(x => x.Value != null).SelectMany(x => x.Value!.Errors).Where(x => !string.IsNullOrEmpty(x.ErrorMessage + " " + x.Exception));
            if (errors.Any())
            {
                BaseResult<string> resp;
                resp = new BaseResult<string>
                {
                    // API 返回的狀態碼
                    Code = StatusCodes.Status406NotAcceptable,
                    // 取得由 API 返回的資料
                    Body = string.Concat(errors.Select(x => x.ErrorMessage))
                };

                context.Result = new JsonResult(resp, new JsonSerializerOptions()
                {
                    Converters = { new JsonStringEnumConverter() }
                });

            }
            base.OnActionExecuting(context);
        }
    }
}
