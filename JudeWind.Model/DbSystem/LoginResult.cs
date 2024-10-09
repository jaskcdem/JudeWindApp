using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JudeWind.Model.DbSystem
{
    /// <summary> 登入結果 </summary>
    public class LoginResult
    {
        /// <summary> 使用者資料 </summary>
        public required UserData User { get; set; }
        public int HttpCode { get; set; }
        public string Message { get; set; } = string.Empty;
    }

    /// <summary> 使用者登入 </summary>
    public class UserLogin
    {
        /// <summary> 使用者ID </summary>
        public required string UserId { get; set; }
        /// <summary> 密碼 </summary>
        public required string Password { get; set; }
        public string? Ip { get; set; }
    }

    /// <summary> 使用者首頁 </summary>
    public class UserData
    {
        /// <summary> 使用者ID </summary>
        public required string UserId { get; set; }
        /// <summary> 姓名 </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary> 權限清單 </summary>
        public List<RoleOutput> Roles { get; set; } = [];
        /// <summary> 授權清單 </summary>
        public List<RolePermissionInput> Permissions { get; set; } = [];
    }
}
