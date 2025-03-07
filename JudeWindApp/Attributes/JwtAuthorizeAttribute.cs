using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Common;
using JudeWind.Service.Register;

namespace JudeWindApp.Attributes
{
    /// <summary></summary>
    public class JwtAuthorizeAttribute(UserInfoService userInfoService) : Attribute, IAuthorizationFilter
    {
        /// <summary></summary>
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var token = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (token == null)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            bool isValid = GeneralTool.VerifyToken(token, aud => userInfoService.IsExist(aud ?? string.Empty).GetAwaiter().GetResult());
            if (!isValid)
            {
                context.Result = new UnauthorizedResult();
            }
        }
    }
}
