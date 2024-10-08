using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAcxess.ProjectContext
{
    /// <summary> 系統代碼類型檔 </summary>
    public partial class Sys_Type
    {
        /// <summary> 類型 </summary>
        [Key]
        [StringLength(50)]
        public required string TYPE { get; set; }
        /// <summary> &#25551;&#36848; </summary>
        [StringLength(300)]
        public required string DESC { get; set; }
        /// <summary> &#26159;&#21542;&#21487;&#32232;&#36655; </summary>
        [Required]
        public bool EDITOR { get; set; }
    }
}
