using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAcxess.ProjectContext
{
    public partial class Sys_SystemSetting
    {
        /// <summary> 實體/虛擬 </summary>
        [Key]
        [StringLength(50)]
        public required string Key { get; set; }
        /// <summary> &#20540; </summary>
        [StringLength(300)]
        public required string Value { get; set; }
        /// <summary> &#39006;&#22411; </summary>
        [Key]
        [StringLength(50)]
        public required string Type { get; set; }
        /// <summary> &#35443;&#32048;&#21517;&#31281; </summary>
        [StringLength(50)]
        public required string Name { get; set; }
        /// <summary> &#24207;&#21015; </summary>
        [StringLength(50)]
        public int Order { get; set; }
        /// <summary> &#26356;&#26032;&#26178;&#38291; </summary>
        [Column(TypeName = "datetime")]
        public DateTime? ModifiedDateTime { get; set; }
        /// <summary> &#26356;&#26032;&#32773; </summary>
        [StringLength(15)]
        public string? ModifiedUser { get; set; }
    }
}
