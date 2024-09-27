using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAcxess.ProjectContext
{
    /// <summary> 系統權限檔 </summary>
    public partial class Sys_RoleGroup
    {
        public Sys_RoleGroup()
        {
            Sys_RoleGroupPermission = [];
            Sys_RoleGroupUser = [];
        }

        /// <summary> 權限Unikey </summary>
        [Key]
        [StringLength(50)]
        public required string Id { get; set; }
        /// <summary> 系統名稱 </summary>
        [Key]
        [StringLength(30)]
        public required string System { get; set; }
        /// <summary> 權限名稱 </summary>
        [Required]
        [StringLength(50)]
        public required string Name { get; set; }
        /// <summary>
        /// &#24314;&#31435;&#26178;&#38291;
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime CreateDateTime { get; set; } = DateTime.Now;
        /// <summary>
        /// &#24314;&#31435;&#32773;
        /// </summary>
        [Required]
        [StringLength(50)]
        public required string CreateUserId { get; set; }
        /// <summary>
        /// &#26356;&#26032;&#26178;&#38291;
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime? UpdateDateTime { get; set; }
        /// <summary>
        /// &#26356;&#26032;&#32773;
        /// </summary>
        [Required]
        [StringLength(50)]
        public string? UpdateUserId { get; set; }

        [InverseProperty("Sys_RoleGroup")]
        public virtual ICollection<Sys_RoleGroupPermission> Sys_RoleGroupPermission { get; set; }
        [InverseProperty("RG_")]
        public virtual ICollection<Sys_RoleGroupUser> Sys_RoleGroupUser { get; set; }
    }
}
