namespace JudeWind.Model.Base
{
    /// <summary> 例外資料模型 </summary>
    public class BaseExceptionMessageModel
    {
        /// <summary> Exception Message </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary> Exception Trace </summary>
        public string StrackTrace { get; set; } = string.Empty;

        /// <summary> Others </summary>
        public string OtherMessage { get; set; } = string.Empty;
    }
}
