using NLog;
using NLog.Config;
using NLog.Targets;
using SD_EXIF_Editor_v2.Services.Interfaces;

namespace SD_EXIF_Editor_v2.Utils
{
    public static class NLogConfigurator
    {
        static NLogConfigurator()
        {
            var config = new LoggingConfiguration();

            var fileTarget = new FileTarget("logfile")
            {
                FileName = "${basedir}/logs/${shortdate}.log"
            };
            config.AddTarget(fileTarget);

            var consoleTarget = new ConsoleTarget("logconsole");
            config.AddTarget(consoleTarget);

            var rule1 = new LoggingRule("*", LogLevel.Warn, consoleTarget);
            config.LoggingRules.Add(rule1);

            var rule2 = new LoggingRule("*", LogLevel.Warn, fileTarget);
            config.LoggingRules.Add(rule2);

            LogManager.Configuration = config;
        }
        public static void UpdateLogLevel(LogLevels logLevel)
        {
            foreach (var rule in LogManager.Configuration.LoggingRules)
            {
                rule.SetLoggingLevels(LogLevel.FromOrdinal((int)logLevel), LogLevel.Fatal);
            }

            LogManager.ReconfigExistingLoggers();
        }
    }
}
