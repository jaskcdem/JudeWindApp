using NLog;
using NLog.Config;
using NLog.Targets;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    public class NLogService
    {
        public void CreateLogger(string logType = "")
        {
            var config = new LoggingConfiguration();
            var fileTarget = new FileTarget
            {
                FileName = "${basedir}/logs/${shortdate}.log",
                Layout = "${date:format=yyyy-MM-dd HH\\:mm\\:ss} [${uppercase:${level}}] ${message}"
            };

            if (!string.IsNullOrEmpty(logType))
            {
                fileTarget.FileName = "${basedir}/logs/" + logType + "/${shortdate}.log";
            }

            config.AddRule(LogLevel.Trace, LogLevel.Error, fileTarget);
            LogManager.Configuration = config;
        }
    }
}
