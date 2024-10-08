namespace JudeWind.Model.DbSystem
{
    /// <summary> 系統代碼 </summary>
    public class SysCodeInput
    {
        /// <summary> 類型 </summary>
        public required string Type { get; set; }
        /// <summary> 上層路徑 </summary>
        public string ParentPath { get; set; } = string.Empty;
        /// <summary> 代碼 </summary>
        public int Code { get; set; }
        /// <summary> 代碼層級 </summary>
        public byte Level { get; set; }
        /// <summary> 內容 </summary>
        public required string Desc { get; set; }
        /// <summary> 操作者Id </summary>
        public required string UserId { get; set; }
    }
    /// <summary> 系統代碼 </summary>
    public class SysCodeOutput
    {
        /// <summary> 類型 </summary>
        public string Type { get; set; } = string.Empty;
        /// <summary> 上層路徑 </summary>
        public string ParentPath { get; set; } = string.Empty;
        /// <summary> 代碼 </summary>
        public int Code { get; set; }
        /// <summary> 代碼層級 </summary>
        public byte Level { get; set; }
        /// <summary> 內容 </summary>
        public string Desc { get; set; } = string.Empty;
    }
}
