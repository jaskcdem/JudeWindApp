using Common;

namespace JudeWindApp.ViewModel
{
    /// <summary></summary>
    public enum NLogLevel
    {
        /// <summary></summary>
        Trace,
        /// <summary></summary>
        Debug,
        /// <summary></summary>
        Info,
        /// <summary></summary>
        Warn,
        /// <summary></summary>
        Error,
        /// <summary></summary>
        Fatal
    }

    /// <summary></summary>
    public class LogResultModel
    {
        /// <summary></summary>
        public required string SystemName { get; set; }
        /// <summary></summary>
        public required string ControllerName { get; set; }
        /// <summary></summary>
        public required string ActionName { get; set; }
        /// <summary></summary>
        public required string IpAddress { get; set; }
        /// <summary></summary>
        public string Source { get; set; } = string.Empty;
        /// <summary></summary>
        public required IHeaderDictionary Header { get; set; }
        /// <summary></summary>
        public DateTime StartTime { get; set; }
        /// <summary></summary>
        public DateTime EndTime { get; set; }
        /// <summary></summary>
        public string Input { get; set; } = string.Empty;
        /// <summary></summary>
        public string Output { get; set; } = string.Empty;
        /// <summary></summary>
        public string UserId { get; set; } = string.Empty;
        /// <summary></summary>
        public string SessionID { get; set; } = string.Empty;
        /// <summary></summary>
        public string Exception { get; set; } = string.Empty;
        /// <summary></summary>
        public double Second => (EndTime - StartTime).TotalSeconds;
    }

    /// <summary>Log File info</summary>
    public class LoggerModel
    {
        /// <summary></summary>
        public LoggerModel()
        {
            Id = GeneralTool.GetRandomStr([GeneralTool.RandomCase.Lower, GeneralTool.RandomCase.Upper, GeneralTool.RandomCase.Number]);
        }
        /// <summary>uuid</summary>
        public string Id { get; private set; }
        /// <summary>Custome code</summary>
        public string Code { get; set; } = string.Empty;
        /// <summary></summary>
        public NLogLevel Level { get; set; } = NLogLevel.Info;
    }
}
