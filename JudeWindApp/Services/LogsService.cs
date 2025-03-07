using JudeWindApp.ViewModel;
using NLog;
using NLog.Web;

namespace JudeWindApp.Services
{
    /// <summary> 登入邏輯 </summary>
    public class LogsService
    {
        private readonly Logger logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();

        #region Methods
        /// <summary>WriteApiLog</summary>
        public async Task<long> WriteApiLog(LogResultModel loggingInfo)
        {
            try
            {
                //var apiLog = loggingInfo.ToModel();
                //var logId = await loginRepository.WriteLog(apiLog);
                //logger.Trace($"Api LogId:{logId}");
                //return logId;
                return 0;
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
                throw;
            }
        }
        /// <summary>WriteApiLog</summary>
        public async Task WriteResult(long logId, string? resultJson)
        {
            try { /*await loginRepository.WriteResult(logId, resultJson);*/ }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
                throw;
            }
        }
        /// <summary>WriteApiLog</summary>
        public async Task WriteException(long logId, string? exception)
        {
            try { /*await loginRepository.WriteException(logId, exception);*/ }
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
