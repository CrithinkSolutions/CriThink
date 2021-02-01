using System;
using FFImageLoading.Helpers;
using MvvmCross.Logging;

namespace CriThink.Client.Core.Logging
{
    public sealed class ImageLoaderLogger : IMiniLogger
    {
        private readonly IMvxLog _log;

        public ImageLoaderLogger(IMvxLogProvider logProvider)
        {
            _log = logProvider?.GetLogFor<ImageLoaderLogger>();
        }

        public void Debug(string message)
        {
            _log?.Debug(message);
        }

        public void Error(string errorMessage)
        {
            _log?.Error(errorMessage);
        }

        public void Error(string errorMessage, Exception exception)
        {
            _log?.ErrorException(errorMessage, exception);
        }
    }
}
