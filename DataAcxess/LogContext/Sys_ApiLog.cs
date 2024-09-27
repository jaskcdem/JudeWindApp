using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAcxess.LogContext
{
    public partial class Sys_ApiLog
    {
        [Key]
        public long LogId { get; set; }
        [StringLength(20)]
        public required string System { get; set; }
        public required string InputData { get; set; }
        [StringLength(4000)]
        public required string Headers { get; set; }
        public required string OutputData { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ActionTime { get; set; }
        [StringLength(100)]
        public required string ControllerName { get; set; }
        [StringLength(100)]
        public required string FunctionName { get; set; }
        [Column(TypeName = "decimal(5, 2)")]
        public decimal? Second { get; set; }
        [StringLength(200)]
        public required string Source { get; set; }
        public string? Exception { get; set; }
        [StringLength(50)]
        public required string SessionId { get; set; }
    }
}
