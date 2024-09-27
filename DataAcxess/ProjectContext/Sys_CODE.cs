using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAcxess.ProjectContext
{
    /// <summary> &#31995;&#32113;&#20195;&#30908;&#27284; </summary>
    public partial class Sys_CODE
    {
        /// <summary> 類型 </summary>
        [Key]
        [StringLength(50)]
        public required string TYPE { get; set; }
        /// <summary> &#36039;&#26009;&#36335;&#24465; </summary>
        [Key]
        [StringLength(30)]
        public required string PARENT_PATH { get; set; }
        /// <summary> &#20195;&#30908; </summary>
        [Key]
        [StringLength(30)]
        public int CODE { get; set; }
        /// <summary>
        /// &#20195;&#30908;&#23652;&#32026;
        /// </summary>
        [Key]
        public byte LEVEL { get; set; }
        /// <summary> &#20839;&#23481; </summary>
        [StringLength(600)]
        public required string DESC { get; set; }
        /// <summary> &#26159;&#21542;&#20316;&#24290; </summary>
        public bool Status { get; set; }
        /// <summary> &#24314;&#31435;&#32773; </summary>
        [StringLength(15)]
        public required string CreateUser { get; set; }
        /// <summary> &#24314;&#31435;&#26178;&#38291; </summary>
        [Column(TypeName = "datetime")]
        public DateTime CreateDatetime { get; set; } = DateTime.Now;
        /// <summary> &#26032;&#22686;&#32773; </summary>
        [StringLength(15)]
        public string? UpdateUser { get; set; }
        /// <summary> &#26356;&#26032;&#26178;&#38291; </summary>
        [Column(TypeName = "datetime")]
        public DateTime? UpdateDatetime { get; set; }
    }
}
