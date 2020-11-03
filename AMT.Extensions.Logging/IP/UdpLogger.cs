// Copyright (c) AltaModa Technologies. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;

namespace AMT.Extensions.Logging.IP
{

    public class UdpLogger : ILogger
    {
        public UdpLogger(string name)
        {
            _name = name;

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
                // Send log message
                Console.WriteLine(formatter.ToString());
                // Write(logLevel, eventId, state, exception, formatter);
                // Write(level, eventId, state, exception, formatter);
            }
            catch (Exception ex)
            {
                // SelfLog.WriteLine($"Failed to write event through {typeof(SerilogLogger).Name}: {ex}");
                throw;
            }
        }

        #endregion ILogger impl
        
        
        readonly string _name;
    }
}