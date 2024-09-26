using Common;

namespace JudeWindApp.ViewModel
{
    /// <summary>  </summary>
    public class LogResultModel
    {
        /// <summary>  </summary>
        public string SystemName { get; set; } = SysSetting.SysName;
        /// <summary>  </summary>
        public string ControllerName { get; set; } = string.Empty;
        /// <summary>  </summary>
        public string ActionName { get; set; } = string.Empty;
        /// <summary>  </summary>
        public string Source { get; set; } = string.Empty;
        /// <summary>  </summary>
        public required IHeaderDictionary Header { get; set; }
        /// <summary>  </summary>
        public DateTime StartTime { get; set; }
        /// <summary>  </summary>
        public DateTime EndTime { get; set; }
        /// <summary>  </summary>
        public string Input { get; set; } = string.Empty;
        /// <summary>  </summary>
        public string Output { get; set; } = string.Empty;
        /// <summary>  </summary>
        public string UserId { get; set; } = string.Empty;
        /// <summary>  </summary>
        public string SessionID { get; set; } = string.Empty;
        /// <summary>  </summary>
        public string Exception { get; set; } = string.Empty;
        /// <summary>  </summary>
        public double Second => (EndTime - StartTime).TotalSeconds;
    }
}
