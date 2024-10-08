using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAcxess.ProjectContext
{
    /// <summary> 使用者總表 </summary>
    public partial class UserInfo
    {
        /// <summary> 使用者ID </summary>
        [Key]
        [StringLength(512)]
        public required string ID { get; set; }
        /// <summary> &#22995;&#21517; </summary>
        [StringLength(512)]
        public required string Name { get; set; }
        /// <summary> &#23494;&#30908; </summary>
        [StringLength(255)]
        public required string Psw { get; set; }
        /// <summary> &#36523;&#20998;&#35657;&#23383;&#34399; </summary>
        [StringLength(15)]
        public string? Idno { get; set; }
        /// <summary> &#26159;&#21542;&#21855;&#29992; </summary>
        public bool? Check { get; set; }
        /// <summary> &#24314;&#31435;&#26178;&#38291; </summary>
        [Column(TypeName = "date")]
        public DateTime CreateDate { get; set; } = DateTime.Now;
        /// <summary> &#21855;&#29992;&#26178;&#38291; </summary>
        [Column(TypeName = "date")]
        public DateTime? CheckDate { get; set; }
        /// <summary> &#26356;&#26032;&#32773; </summary>
        [StringLength(512)]
        public string? UpdateUser { get; set; }
        /// <summary> &#26356;&#26032;&#26178;&#38291; </summary>
        [Column(TypeName = "datetime")]
        public DateTime? UpdateDataTime { get; set; }
        [StringLength(50)]
        public required string LastLoginIP { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? LastLoginTime { get; set; }
        public short? LoginFailedCount { get; set; }
        [StringLength(254)]
        public required string Email { get; set; }
    }
}
