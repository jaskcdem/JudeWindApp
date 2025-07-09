using JudeWindApp.ViewModel;
using Newtonsoft.Json;
using NLog;
using System.Text;

namespace JudeWindApp.Services
{
    /// <summary>NLog Service</summary>
    public class LogsService
    {
        private readonly Logger errlog = LogManager.GetLogger("ErrorLog");

        #region Write Log
        /// <summary>Write Api Log</summary>
        /// <returns>Log uuid</returns>
        public string WriteLog(LoggerModel logger, string? message)
        {
            try
            {
                Logging(logger, $"{logger.Id} {logger.Code} | {message}");
                return logger.Id;
            }
            catch (Exception ex)
            {
                errlog.Error($"Write Log Fail: {ex}");
                throw;
            }
        }
        /// <summary>Write Api Log</summary>
        /// <returns>Log uuid</returns>
        public async Task<string> WriteApiLog(LoggerModel logger, LogResultModel info)
        {
            //Exclude some headers from log
            List<string> logExclude = ["__StartTime__", "__InputData__", "controllerName", "actionName", "Host", "clientid"];
            try
            {
                StringBuilder msg = new($@"{logger.Id} {logger.Code} | {info.SystemName}/{info.ControllerName}/{info.ActionName}
{nameof(info.StartTime)}: {info.StartTime:yyyy-MM-dd HH:mm:ss}, {nameof(info.EndTime)}: {info.EndTime:yyyy-MM-dd HH:mm:ss}, Spent: {info.Second} seconds
{nameof(info.Source)}: {info.Source}, Session: {info.SessionID}, Ip: {info.IpAddress}, User: {info.UserId},
{nameof(info.Input)}: {info.Input}, Header: ");
                var headerInfo = info.Header.Where(h => !logExclude.Contains(h.Key) && !string.IsNullOrWhiteSpace(h.Value))
                    .Select(x => new
                    {
                        Column = x.Key,
                        Value = x.Value.ToString()
                    }).ToList();
                msg.Append($"{JsonConvert.SerializeObject(headerInfo)}, ");
                msg.Append($"{nameof(info.Output)}: {info.Output}");
                Logging(logger, msg.ToString());
                await Task.CompletedTask;
                return logger.Id;
            }
            catch (Exception ex)
            {
                errlog.Error($"Write Api Log Fail: {ex}");
                throw;
            }
        }
        /// <summary>WriteApiLog</summary>
        /// <returns>Log uuid</returns>
        public async Task<string> WriteResult(LoggerModel logger, object? data)
        {
            try
            {
                Logging(logger, $"{logger.Id} {logger.Code} | {JsonConvert.SerializeObject(data)}");
                await Task.CompletedTask;
                return logger.Id;
            }
            catch (Exception ex)
            {
                errlog.Error($"Write Json Log Fail: {ex}");
                throw;
            }
        }
        /// <summary>WriteApiLog</summary>
        /// <returns>Log uuid</returns>
        public async Task<string> WriteException(LoggerModel logger, Exception exception)
        {
            try
            {
                Logging(logger, $"{logger.Id} {logger.Code} | {exception}");
                await Task.CompletedTask;
                return logger.Id;
            }
            catch (Exception ex)
            {
                errlog.Error($"Write Exception Log Fail: {ex}");
                throw;
            }
        }

        private void Logging(LoggerModel logger, string message)
        {
            switch (logger.Level)
            {
                case NLogLevel.Trace: errlog.Trace(message); break;
                case NLogLevel.Debug: errlog.Debug(message); break;
                case NLogLevel.Info: errlog.Info(message); break;
                case NLogLevel.Warn: errlog.Warn(message); break;
                case NLogLevel.Error: errlog.Error(message); break;
                case NLogLevel.Fatal: errlog.Fatal(message); break;
            }
        }
        #endregion

        /// <summary>Get domain from url</summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public string GetSourceOrigin(string url)
        {
            url = url.Replace("http://", "").Replace("https://", "");
            return url.Split("/".ToCharArray())[0];
        }
    }
}
