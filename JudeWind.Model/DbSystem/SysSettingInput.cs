namespace JudeWind.Model.DbSystem
{
    /// <summary> 系統設定 </summary>
    public class SysSettingInput
    {
        /// <summary> 設定Id </summary>
        public required SysSettingPKey PKey { get; set; }
        /// <summary> 值 </summary>
        public required string Value { get; set; }
        /// <summary> 詳細名稱 </summary>
        public required string Name { get; set; }
        /// <summary> 序列 </summary>
        public int Order { get; set; }
        /// <summary> 操作者Id </summary>
        public required string UserId { get; set; }
    }
    /// <summary> 系統設定 </summary>
    public class SysSettingOutput
    {
        /// <summary> 實體/虛擬 </summary>
        public string Key { get; set; } = string.Empty;
        /// <summary> 值 </summary>
        public string Value { get; set; } = string.Empty;
        /// <summary> 類型 </summary>
        public string Type { get; set; } = string.Empty;
        /// <summary> 詳細名稱 </summary>
        public string Name { get; set; } = string.Empty;
    }

    /// <summary> 系統設定Id </summary>
    public class SysSettingPKey
    {
        /// <summary> 實體/虛擬 </summary>
        public string Key { get; set; } = string.Empty;
        /// <summary> 類型 </summary>
        public string Type { get; set; } = string.Empty;
    }
}
