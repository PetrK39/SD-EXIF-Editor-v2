using NLog;
using SD_EXIF_Editor_v2.Services.Interfaces;

namespace SD_EXIF_Editor_v2.Utils
{
    public static class NLogConfigurator
    {
        public static void UpdateLogLevel(LogLevels logLevel)
        {
            LogManager.Configuration.Variables["logLevel"] = logLevel.ToString();
            LogManager.ReconfigExistingLoggers();
        }
    }
}
