using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAcxess.ProjectContext
{
    public partial class Sys_RoleGroupPermission
    {
        /// <summary> 權限群組控制流水號 </summary>
        [Key]
        public int Id { get; set; }
        /// <summary> &#27402;&#38480;&#32676;&#32068; </summary>
        [Required]
        [StringLength(50)]
        public required string RGId { get; set; }
        /// <summary> &#31995;&#32113;&#21517;&#31281; </summary>
        [Required]
        [StringLength(30)]
        public required string MFSystem { get; set; }
        /// <summary> &#31995;&#32113;&#38917;&#30446; </summary>
        [Required]
        [StringLength(50)]
        public required string MFFuncId { get; set; }

        [ForeignKey("RGId,MFSystem")]
        [InverseProperty("Sys_RoleGroupPermission")]
        public required virtual Sys_RoleGroup Sys_RoleGroup { get; set; }
    }
}
