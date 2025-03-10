﻿using Common;
using JudeWindApp.Attributes;
using Microsoft.AspNetCore.Mvc;
using MimeKit;

namespace JudeWindApp.Controllers
{
#if !DEBUG
    [JwtAuthorizeAttribute]
#endif
    /// <summary>  </summary>
    [Route("[controller]/[action]")]
    [ApiController]
    [TypeFilter(typeof(AppLogFilterAttribute), Arguments = ["WebApi"])]
    [TypeFilter(typeof(RolePermissionVaildAttribute))]
    [TypeFilter(typeof(AppResultFilter))]
    [TypeFilter(typeof(ValidateModelAttribute))]
    public class BaseApiController : ControllerBase
    {
        #region Public Members
        /// <summary> 使用者帳號 </summary>
        public string UserId => GetUserId();

        /// <summary>  </summary>
        public string IPAddress => Request.HttpContext.Connection.RemoteIpAddress?.ToString() ?? SysSetting.DefaultIp;
        #endregion

        /// <summary>  </summary>
        protected IActionResult DownloadFile(string virtualPath) => File(virtualPath, MimeTypes.GetMimeType(virtualPath), Path.GetFileName(virtualPath));
        /// <summary>  </summary>
        protected IActionResult DownloadPhysicalFile(string physicalPath) => PhysicalFile(physicalPath, MimeTypes.GetMimeType(physicalPath), Path.GetFileName(physicalPath));

        /// <summary>  </summary>
        protected IActionResult DownloadFileStream(Stream fileStream, string fileName) => File(fileStream, MimeTypes.GetMimeType(fileName), fileName);
        /// <summary>  </summary>
        protected IActionResult DownloadFileStream(Stream fileStream, string mimeType, string fileName) => File(fileStream, mimeType, fileName);
        /// <summary>  </summary>
        protected IActionResult DownloadFileContent(byte[] fileBytes, string fileName) => File(fileBytes, MimeTypes.GetMimeType(fileName), fileName);
        /// <summary>  </summary>
        protected IActionResult DownloadFileContent(byte[] fileBytes, string mimeType, string fileName) => File(fileBytes, mimeType, fileName);

        #region Private Methods
        /// <summary> 取回使用者帳號 </summary>
        private string GetUserId()
        {
            var result = string.Empty;
            try
            {
                var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId");
                if (userIdClaim != null)
                    result = userIdClaim.Value;
            }
            catch (Exception) { throw; }
            return result;
        }
        #endregion
    }
}
