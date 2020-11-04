// Copyright (c) AltaModa Technologies. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft.Extensions.Logging;


namespace AMT.Extensions.Logging.IP
{

    public class UdpLogger : ILogger
    {
        internal UdpLogger(string name, UdpLoggerProcessor processor)
        {
            if (null == name)  { throw new ArgumentNullException(nameof(name)); }
            _name = name;
            _processor = processor;

            // see https://github.com/serilog/serilog-extensions-logging/blob/dev/src/Serilog.Extensions.Logging/Extensions/Logging/SerilogLogger.cs
            //  re: adding logger to context
        }



        #region ILogger impl


        /// <inheritdoc />
        public IDisposable BeginScope<TState>(TState state)
        {
            return NullScope.Instance;
        }

        /// <inheritdoc />
        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel != LogLevel.None;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {

            if (!IsEnabled(logLevel))
            {
                return;
            }

            try
            {
                // TODO: require LogMessageEntry in params?
                var entry = new LogMessageEntry {
                    LogLevel = logLevel,
                    EventId = eventId,
                    Message = formatter.Invoke(state, exception)
                };
                // Send log message
                _processor.EnqueueMessage(entry);
            }
            catch (Exception )
            {
                // SelfLog.WriteLine($"Failed to write event through {typeof(SerilogLogger).Name}: {ex}");
                throw;
            }
        }

        #endregion ILogger impl
        
        private readonly UdpLoggerProcessor _processor;        
        private readonly string _name;
    }
}