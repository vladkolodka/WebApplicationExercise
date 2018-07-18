using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Web.Http.Tracing;
using NLog;

namespace WebApplicationExercise.Services.Logging
{
    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public sealed class NLogger : ITraceWriter
    {
        private static readonly Logger ClassLogger = LogManager.GetCurrentClassLogger();

        private static readonly Lazy<Dictionary<TraceLevel, Action<string>>> LoggingMap =
            new Lazy<Dictionary<TraceLevel, Action<string>>>(() => new Dictionary<TraceLevel, Action<string>>
            {
                {TraceLevel.Info, ClassLogger.Info},
                {TraceLevel.Debug, ClassLogger.Debug},
                {TraceLevel.Error, ClassLogger.Error},
                {TraceLevel.Fatal, ClassLogger.Fatal},
                {TraceLevel.Warn, ClassLogger.Warn}
            });

        private static Dictionary<TraceLevel, Action<string>> Logger => LoggingMap.Value;

        /// <inheritdoc />
        public void Trace(HttpRequestMessage request, string category, TraceLevel level,
            Action<TraceRecord> traceAction)
        {
            if (level == TraceLevel.Off) return;

            var record = new TraceRecord(request, category, level);
            traceAction(record);
            Log(record);
        }

        private static void Log(TraceRecord record)
        {
            var message = new StringBuilder();

            if (record.Request != null)
            {
                if (record.Request.Method != null)
                    message.Append(record.Request.Method);

                if (record.Request.RequestUri != null)
                    message.Append(" ").Append(record.Request.RequestUri);
            }

            if (!string.IsNullOrWhiteSpace(record.Category))
                message.Append(" ").Append(record.Category);

            if (!string.IsNullOrWhiteSpace(record.Operator))
                message.Append(" ").Append(record.Operator).Append(" ").Append(record.Operation);

            if (!string.IsNullOrWhiteSpace(record.Message))
                message.Append(" ").Append(record.Message);

            if (!string.IsNullOrWhiteSpace(record.Exception?.GetBaseException().Message))
                message.Append(" ").Append(record.Exception.GetBaseException().Message);

            Logger[record.Level](message.ToString());
        }
    }
}