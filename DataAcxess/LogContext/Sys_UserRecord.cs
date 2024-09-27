using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAcxess.LogContext
{
    public partial class Sys_UserRecord
    {
        /// <summary> 使用者 </summary>
        [Key]
        [StringLength(50)]
        public required string UserId { get; set; }
        /// <summary> 記錄時間 </summary>
        [Key]
        [Column(TypeName = "datetime")]
        public DateTime UR_RecordDateTime { get; set; }
        /// <summary> 紀錄API </summary>
        [Key]
        [StringLength(50)]
        public required string RecordEvent { get; set; }
        public required string InputData { get; set; }
        [StringLength(40)]
        public required string IP { get; set; }
    }
}
