using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAcxess.ProjectContext
{
    public partial class Sys_RoleGroupUser
    {
        /// <summary> 權限群組 </summary>
        [Key]
        [StringLength(50)]
        public required string RGId { get; set; }
        /// <summary> &#31995;&#32113;&#21517;&#31281; </summary>
        [Key]
        [StringLength(30)]
        public required string RGSystem { get; set; }
        /// <summary> &#27402;&#38480;&#32676;&#32068;&#20154;&#21729; </summary>
        [Key]
        [StringLength(50)]
        public required string UserId { get; set; }

        [ForeignKey("RGId,RGSystem")]
        [InverseProperty(nameof(Sys_RoleGroup.Sys_RoleGroupUser))]
        public required virtual Sys_RoleGroup RG_ { get; set; }
    }
}
