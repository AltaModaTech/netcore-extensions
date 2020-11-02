// Copyright (c) AltaModa Technologies. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AMT.Extensions.Logging.IP
{

    public class UdpLogger : ILogger
    {

        public UdpLogger(UdpLoggerProvider provider, ILogger logger = null, string category = null)
        {
            _provider = provider ?? throw new ArgumentNullException(nameof(provider));
            _logger = logger;

            // see https://github.com/serilog/serilog-extensions-logging/blob/dev/src/Serilog.Extensions.Logging/Extensions/Logging/SerilogLogger.cs
            //  re: adding logger to context

            if (null != category)
            {
                _logger = _logger.ForContext(Constants.SourceContextPropertyName, category);
            }
        }



        #region ILogger impl

        public IDisposable BeginScope<TState>(TState state)
        {
            return _provider.BeginScope(state);
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return _logger.IsEnabled(logLevel);
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            var level = LevelConvert.ToSerilogLevel(logLevel);
            if (!_logger.IsEnabled(level))
            {
                return;
            }

            try
            {
                Write(level, eventId, state, exception, formatter);
            }
            catch (Exception ex)
            {
                SelfLog.WriteLine($"Failed to write event through {typeof(SerilogLogger).Name}: {ex}");
            }
        }

        #endregion ILogger impl


        readonly ILogger _logger;
        readonly ILoggerProvider _provider;
    }
}