namespace JudeWind.Model.DbSystem
{
    /// <summary> 系統代碼類型 </summary>
    public class SysTypeInput
    {
        /// <summary> 類型 </summary>
        public required string Type { get; set; }
        /// <summary> 描述 </summary>
        public required string Desc { get; set; }
        /// <summary> 是否可編輯 </summary>
        public bool Editor { get; set; } = true;
    }
    /// <summary> 系統代碼類型 </summary>
    public class SysTypeOutput
    {
        /// <summary> 類型 </summary>
        public string Type { get; set; } =string.Empty;
        /// <summary> 描述 </summary>
        public string Desc { get; set; } =string.Empty;
        /// <summary> 是否可編輯 </summary>
        public bool Editor { get; set; } = true;
    }
}
