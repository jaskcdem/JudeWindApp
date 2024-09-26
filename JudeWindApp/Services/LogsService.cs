using Common;
using JudeWindApp.ViewModel;
using NLog;
using System;
using System.Threading.Tasks;

namespace JudeWindApp.Services
{
    /// <summary> 登入邏輯 </summary>
    public class LogsService
    {
        #region Private Members
        //private readonly LogContext _context;
        private NLogService _logService = new();
        Logger logger = LogManager.GetCurrentClassLogger();
        #endregion

        #region Constructor
        ///// <summary>  </summary>
        //public LogsService(LogContext context)
        //{
        //    _context = context;
        //    _logService.CreateLogger();
        //}
        /// <summary>  </summary>
        public LogsService()
        {
            _logService.CreateLogger();
        }
        #endregion

        #region Public Methods
        /// <summary> WriteApiLog </summary>
        /// <param name="logResult"></param>
        /// <returns></returns>
        public async Task WriteApiLog(LogResultModel logResult)
        {
            try
            {
                //write log
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
                throw;
            }
        }

        /// <summary> 將取得的網址，取出網域 </summary>
        /// <param name="strOrigin"></param>
        /// <returns></returns>
        public string GetSourceOrigin(string strOrigin)
        {
            strOrigin = strOrigin.Replace("http://", "").Replace("https://", "");
            return strOrigin.Split("/".ToCharArray())[0];
        }
        #endregion
    }
}
