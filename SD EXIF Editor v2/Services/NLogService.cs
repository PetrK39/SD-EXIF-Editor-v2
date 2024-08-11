using NLog;
using SD_EXIF_Editor_v2.Services.Interfaces;

namespace SD_EXIF_Editor_v2.Services
{
    class NLogService : ILoggingService
    {
        private readonly ILogger logger;
        public NLogService(ILogger logger)
        {
            this.logger = logger;
        }

        public void Trace(string message)
        {
            logger.Trace(message);
        }

        public void Debug(string message)
        {
            logger.Debug(message);
        }

        public void Info(string message)
        {
            logger.Info(message);
        }

        public void Warn(string message)
        {
            logger.Warn(message);
        }

        public void Error(string message, Exception? exception = null)
        {
            logger.Error(exception, message);
        }

        public void Fatal(string message, Exception? exception = null)
        {
            logger.Fatal(exception, message);
        }
    }
}
