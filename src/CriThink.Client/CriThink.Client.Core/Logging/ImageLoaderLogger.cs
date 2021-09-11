using System;
using FFImageLoading.Helpers;
using ILogger = Serilog.ILogger;

namespace CriThink.Client.Core.Logging
{
    public sealed class ImageLoaderLogger : IMiniLogger
    {
        private readonly ILogger _logger;

        public ImageLoaderLogger(ILogger logger)
        {
            _logger = logger;
        }

        public void Debug(string message)
        {
            _logger?.Debug(message);
        }

        public void Error(string errorMessage)
        {
            _logger?.Error(errorMessage);
        }

        public void Error(string errorMessage, Exception exception)
        {
            _logger?.Error(exception, errorMessage);
        }
    }
}
