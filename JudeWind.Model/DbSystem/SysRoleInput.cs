using Common;
using System.ComponentModel.DataAnnotations;

namespace JudeWind.Model.DbSystem
{
    /// <summary> 系統權限 </summary>
    public class SysRoleInput
    {
        /// <summary> 權限群組id </summary>
        public string RGId { get; set; } = string.Empty;
        /// <summary> 權限名稱 </summary>
        public required string Name { get; set; }
        /// <summary> 操作者Id </summary>
        public required string UserId { get; set; }
    }

    /// <summary> 使用者權限群組 </summary>
    public class SysRoleUserInput
    {
        /// <summary> 使用者Id </summary>
        public required string UserId { get; set; }
        /// <summary> 權限群組id </summary>
        public List<string> RGId { get; set; } = [];
    }

    /// <summary> 系統授權 </summary>
    public class RolePermissionInput
    {
        /// <summary> 系統項目 </summary>
        public required string MFSystem { get; set; }
        /// <summary> 系統名稱 </summary>
        public required string MFFuncId { get; set; }
    }

    /// <summary> 權限群組控制 </summary>
    public class SysRolePermissionInput
    {
        /// <summary> 權限群組id </summary>
        public required string RGId { get; set; }
        /// <summary> 授權清單 </summary>
        public List<RolePermissionInput> Permissions { get; set; } = [];
    }

    /// <summary> 系統權限清單 </summary>
    public class RoleOutput
    {
        /// <summary> 權限群組id </summary>
        public string RGId { get; set; } = string.Empty;
        /// <summary> 權限名稱 </summary>
        public string Name { get; set; } = string.Empty;
    }

    /// <summary> 使用者權限清單 </summary>
    public class RoleUserOutput
    {
        /// <summary> 使用者Id </summary>
        public required string UserId { get; set; }

        /// <summary> 權限清單 </summary>
        public List<RoleOutput> Roles { get; set; } = [];
    }

    /// <summary> 授權清單 </summary>
    public class RolePermissionOutput
    {
        /// <summary> 權限群組id </summary>
        public string RGId { get; set; } = string.Empty;
        /// <summary> 權限名稱 </summary>
        public string Name { get; set; } = string.Empty;
        /// <summary> 系統項目 </summary>
        public string MFSystem { get; set; } = string.Empty;
        /// <summary> 系統名稱 </summary>
        public string MFFuncId { get; set; } = string.Empty;
    }

    /// <summary> 使用者授權清單 </summary>
    public class UserPermissionOutput
    {
        /// <summary> 使用者Id </summary>
        public required string UserId { get; set; }

        /// <summary> 授權清單 </summary>
        public List<RolePermissionOutput> Permissions { get; set; } = [];
    }
}
