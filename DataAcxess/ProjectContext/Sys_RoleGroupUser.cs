using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAcxess.ProjectContext
{
    /// <summary> 使用者權限群組檔 </summary>
    public partial class Sys_RoleGroupUser
    {
        /// <summary> 權限群組 </summary>
        [Key]
        [StringLength(50)]
        public required string RGId { get; set; }
        /// <summary> &#27402;&#38480;&#32676;&#32068;&#20154;&#21729; </summary>
        [Key]
        [StringLength(512)]
        public required string UserId { get; set; }

        [ForeignKey("RGId")]
        [InverseProperty(nameof(Sys_RoleGroup.Sys_RoleGroupUser))]
        public required virtual Sys_RoleGroup RG_ { get; set; }
    }
}
