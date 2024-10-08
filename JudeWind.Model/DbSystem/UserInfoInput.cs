using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JudeWind.Model.DbSystem
{
    /// <summary> 使用者 </summary>
    public class UserInfoInput
    {
        /// <summary> 使用者ID </summary>
        public required string UserId { get; set; }
        /// <summary> 姓名 </summary>
        public required string Name { get; set; }
        /// <summary> 密碼 </summary>
        public required string Psw { get; set; }
        /// <summary> 身分證字號 </summary>
        public string? Idno { get; set; }
        public required string IP { get; set; }
        public string? Email { get; set; }
        /// <summary> 操作者ID </summary>
        public string? ModifyUserId { get; set; }
    }
    /// <summary> 使用者 </summary>
    public class UserInfoOutput
    {
        /// <summary> 使用者ID </summary>
        public required string UserId { get; set; }
        /// <summary> 姓名 </summary>
        public string Name { get; set; } = string.Empty;
        /// <summary> 身分證字號 </summary>
        public string? Idno { get; set; }
        public string? Email { get; set; }
    }

    /// <summary> 使用者啟用/關閉 </summary>
    public class UserApplyCheck
    {
        /// <summary> 使用者ID </summary>
        public required string UserId { get; set; }
        /// <summary> 是否啟用 </summary>
        public bool Check { get; set; }
    }
    /// <summary> 使用者變更密碼 </summary>
    public class ChangePswInput
    {
        /// <summary> 使用者ID </summary>
        public required string UserId { get; set; }
        /// <summary> 舊密碼 </summary>
        public required string OldPassward { get; set; }
        /// <summary> 新密碼 </summary>
        public required string NewPassward { get; set; }
    }
    /// <summary> 使用者忘記密碼 </summary>
    public class ForgetPswResult
    {
        /// <summary> 使用者ID </summary>
        public required string UserId { get; set; }
        /// <summary> 新密碼 </summary>
        public string NewPassward { get; set; } = string.Empty;
    }
}
