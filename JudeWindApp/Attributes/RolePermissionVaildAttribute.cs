using JudeWind.Service.Register;
using JudeWindApp.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

/* 在Controller啟動階段便進行角色授權檢核
 * 一度無法抓取到Attribute,但是經過逐行debug後
 * 終於找到方法抓取Attribute出來進行檢核
 * ===============================================
 * 免登入便可用的controller或method, 添加[AllowAnonymousAttribute]
 * 需登入但所有角色都可用的, 添加[OpenPermissionAttribute]
 * 需登入但僅限制角色, 而非permission的, 添加[OpenPermissionAttribute]並提供角色Id白名單
 */
namespace JudeWindApp.Attributes
{
    /// <summary>權限檢核Attribute </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class RolePermissionVaildAttribute(UserInfoService userInfoService) : ActionFilterAttribute
    {
        #region Private Members
        UserInfoService _userInfoService = userInfoService;
        /// <summary> MenuFunc(key) - Controller(value) </summary>
        readonly Dictionary<string, string> MenuFuncControllerPairs = new()
        {
            { "Home", string.Empty },
            { "Dashboard", "Dashboard,WarehouseManagement" },
            { "Case", string.Empty },
            { "FileManagement", string.Empty },
            { "FileUpload", "FileUpload,WarehouseManagement" },
            { "System", string.Empty },
            { "UserManagement", "UserInfo" },
            { "UserLogRecord", "LogAudit" },
        };
        #endregion

        #region Methods
        /// <summary>  </summary>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            try
            {
                if (!IsVaild(context))
                {
                    context.Result = new StatusCodeResult(StatusCodes.Status401Unauthorized);
                }
            }
            catch (Exception) { throw; }
        }
        bool IsVaild(ActionExecutingContext context)
        {
            var controllerName = ((ControllerBase)context.Controller).ControllerContext.ActionDescriptor.ControllerName;
            var actionName = ((ControllerBase)context.Controller).ControllerContext.ActionDescriptor.ActionName;
            var _userid = ((BaseApiController)context.Controller).UserId;
            if (string.IsNullOrEmpty(_userid))
            {
#if DEBUG
                //_userid = "Administrator";
                return true;
#endif
            }
            var _permission = _userInfoService.GetUserPermission(_userid).Result;
            //取得非繼承的Attribute
            var _methodAttrs = ((ControllerBase)context.Controller).ControllerContext.ActionDescriptor.MethodInfo.GetCustomAttributes(false);
            var _classAttrs = ((ControllerBase)context.Controller).ControllerContext.ActionDescriptor.ControllerTypeInfo.GetCustomAttributes(false);
            if (_methodAttrs.Any(x => x is AllowAnonymousAttribute) || _classAttrs.Any(x => x is AllowAnonymousAttribute))
                return true;
            //如果有掛OpenPermissionAttribute,則改檢查使用者RoleId是否在白名單內
            if (_methodAttrs.Any(x => x is OpenPermissionAttribute))
            {
                OpenPermissionAttribute _openPermission = (OpenPermissionAttribute)_methodAttrs.FirstOrDefault(x => x is OpenPermissionAttribute)!;
                return _openPermission.IsAllow(_userInfoService, _userid);
            }
            else if (_classAttrs.Any(x => x is OpenPermissionAttribute))
            {
                OpenPermissionAttribute _openPermission = (OpenPermissionAttribute)_classAttrs.FirstOrDefault(x => x is OpenPermissionAttribute)!;
                return _openPermission.IsAllow(_userInfoService, _userid);
            }
            else
            {
                var _permissionController = MenuFuncControllerPairs.Where(x => _permission.Contains(x.Key)).SelectMany(x => x.Value.Split(','));
                return _permissionController.Contains(controllerName);
            }
        }
        #endregion
    }

    /// <summary> 權限檢核特例公開Attribute </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class OpenPermissionAttribute : Attribute
    {
        /// <summary> 權限檢核特例公開Attribute </summary>
        public OpenPermissionAttribute() { }
        /// <summary> 權限檢核特例公開Attribute </summary>
        public OpenPermissionAttribute(params string[] rolesId) { AllowRoleIdList.AddRange(rolesId); }

        readonly List<string> AllowRoleIdList = [];

        /// <summary>  </summary>
        public bool IsAllow(UserInfoService _userInfoService, string userid)
        {
            if (AllowRoleIdList.Count == 0) return true;
            foreach (string roleId in AllowRoleIdList)
            {
                if (_userInfoService.IsRole(userid, roleId).Result) return true;
            }
            return false;
        }
    }
}
