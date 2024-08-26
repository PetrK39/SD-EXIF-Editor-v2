using NLog;
using SD_EXIF_Editor_v2.Services.Interfaces;

namespace SD_EXIF_Editor_v2.Utils
{
    public static class NLogConfigurator
    {
        static NLogConfigurator()
        {
            var config = new NLog.Config.LoggingConfiguration();

            var logfile = new NLog.Targets.FileTarget("logfile") { FileName = "${basedir}/logs/${shortdate}.log" };
            var console = new NLog.Targets.ConsoleTarget("logconsole");

            var nlogLevel = LogLevel.FromOrdinal(Properties.Settings.Default.LogLevel);

            config.AddRule(nlogLevel, LogLevel.Fatal, logfile);
            config.AddRule(nlogLevel, LogLevel.Fatal, console);

            NLog.LogManager.Configuration = config;
        }
        public static void UpdateLogLevel(LogLevels logLevel)
        {
            var configuration = LogManager.Configuration;

            foreach (var rule in configuration.LoggingRules)
            {
                var nlogLevel = LogLevel.FromOrdinal((int)logLevel);

                rule.DisableLoggingForLevels(LogLevel.Trace, LogLevel.Fatal);
                rule.EnableLoggingForLevels(nlogLevel, LogLevel.Fatal);
            }

            LogManager.ReconfigExistingLoggers();
        }
    }
}
