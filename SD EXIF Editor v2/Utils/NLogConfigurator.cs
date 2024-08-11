using NLog;
using SD_EXIF_Editor_v2.Services.Interfaces;

namespace SD_EXIF_Editor_v2.Utils
{
    public static class NLogConfigurator
    {
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
