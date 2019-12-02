using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ophelia.Diagnostics
{
    public class EventLogger : ILogger
    {
        private static readonly Lazy<ILogger> instance = new Lazy<ILogger>();
        public const string Name = "EventLogger";
        private EventLog eventLog;
        public EventLogger(EventLog eventLog)
        {
            this.eventLog = eventLog;
        }

        public void Information(string message)
        {
            eventLog.WriteEntry(message);
        }

        public void Error(Exception ex)
        {
            if (ex == null)
                return;

            var message = string.Format("Message: {0} \r\n StackTrace: {1} \r\n InnerException: {2}", ex, ex.StackTrace, ex.InnerException);
            eventLog.WriteEntry(message, EventLogEntryType.Error);
        }

        public static ILogger Current
        {
            get { return instance.Value; }
        }

        public void Exclamation(string message)
        {
            eventLog.WriteEntry(message, EventLogEntryType.Warning);
        }
    }
}
