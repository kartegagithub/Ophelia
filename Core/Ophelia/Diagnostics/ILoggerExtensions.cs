using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ophelia.Diagnostics
{
    public static class ILoggerExtensions
    {
        public static void Exclamation(this ILogger logger, string messageFormat, params object[] args)
        {
            Guard.ArgumentNullException(logger, "logger");
            
            Guard.StringNullOrEmptyException(messageFormat, "messageFormat");

            var message = string.Format(messageFormat, args);
            logger.Exclamation(message);
        }

        public static void Information(this ILogger logger, string messageFormat, params object[] args)
        {
            Guard.ArgumentNullException(logger, "logger");

            Guard.StringNullOrEmptyException(messageFormat, "messageFormat");

            var message = string.Format(messageFormat, args);
            logger.Information(message);
        }
    }
}
