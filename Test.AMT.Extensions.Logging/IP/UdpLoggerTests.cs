// Copyright (c) AltaModa Technologies. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Ext = AMT.Extensions.Logging.IP;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Xunit;
using System.Net;
using System.Runtime.CompilerServices;


namespace Test.AMT.Extensions.Logging.IP
{
    [ExcludeFromCodeCoverage]
    public class UdpLoggerTests
    {

        [Fact]
        public void verify_each_loglevel()
        {
            // Prepare listener to receive messages; isolate port for message count
            var opts = new Ext.UdpLoggerOptions(new IPEndPoint(IPAddress.Loopback, 17460));

            using (_receiver = new UdpReceiver())
            {
                _receiver.Start(opts);

                // Add options to provider before create logger
                using (_provider = new Ext.UdpLoggerProvider(opts)) {
                    _logger = _provider.CreateLogger("test") as Ext.UdpLogger;

                    // Log a message of each LogLevel
                    foreach (int level in _logLevels)
                    {
                        _logger.Log((LogLevel)level, DEFAULT_EVENTID, DEFAULT_STATE, DEFAULT_EXCEPTION, setStringFormatter);
                    }

                    // Brief wait, then stop listener
                    System.Threading.Thread.Sleep(100);
                    _receiver.Stop();

                    // NOTE: RetrieveMessages returns a _consuming_ enumerator, so it can only be used once.
                    //  LogLevel.None filtered, so reduce by 1.
                    _receiver.RetrieveMessages().Count().Should().Be(_logLevels.Length - 1);
                }
            }
        }


        [Fact]
        public void verify_logs_with_null_data()
        {
            // Prepare listener to receive messages
            var opts = new Ext.UdpLoggerOptions();

            using (_receiver = new UdpReceiver())
            {
                _receiver.Start(opts);

                // Add options to provider before create logger
                _provider = new Ext.UdpLoggerProvider(opts);
                var logger = _provider.CreateLogger("test") as Ext.UdpLogger;

                // Log non-null data
                logger.Log(GetRandomLogLevel(), DEFAULT_EVENTID, DEFAULT_STATE, DEFAULT_EXCEPTION, setStringFormatter);

                // log null data
                logger.Log(GetRandomLogLevel(), DEFAULT_EVENTID, NULL_STATE, DEFAULT_EXCEPTION, setStringFormatter);
                logger.Log(GetRandomLogLevel(), DEFAULT_EVENTID, DEFAULT_STATE, NULL_EXCEPTION, setStringFormatter);

                // Log non-null data
                logger.Log(GetRandomLogLevel(), DEFAULT_EVENTID, DEFAULT_STATE, DEFAULT_EXCEPTION, setStringFormatter);
                
                // Brief wait & stop listener
                System.Threading.Thread.Sleep(100);
                _receiver.Stop();

                // NOTE: RetrieveMessages returns a _consuming_ enumerator, so it can only be used once.
                _receiver.RetrieveMessages().Count().Should().Be(4);  // -1 since None shouldn't be logged
            }
        }


        [Fact]
        public void verify_few_log_messages()
        {
            // Prepare listener to receive messages
            var opts = new Ext.UdpLoggerOptions();
            testLog.WriteLine($"UDP options: {opts.IPEndPoint.ToString()}");

            using (_receiver = new UdpReceiver())
            {
                _receiver.Start(opts);

                // Add options to provider before create logger
                _provider = new Ext.UdpLoggerProvider(opts);
                var logger = _provider.CreateLogger("test") as Ext.UdpLogger;

                // Log some messages
                logger.Log(LogLevel.Debug, DEFAULT_EVENTID, DEFAULT_STATE, DEFAULT_EXCEPTION, setStringFormatter);
                logger.Log(LogLevel.Debug, DEFAULT_EVENTID, NULL_STATE, DEFAULT_EXCEPTION, setStringFormatter);
                logger.Log(LogLevel.Debug, DEFAULT_EVENTID, DEFAULT_STATE, NULL_EXCEPTION, setStringFormatter);
                // LogLevel None results is no log
                logger.Log(LogLevel.None, DEFAULT_EVENTID, DEFAULT_STATE, NULL_EXCEPTION, setStringFormatter);

                // Brief wait & stop listener
                System.Threading.Thread.Sleep(100);
                _receiver.Stop();


                // NOTE: RetrieveMessages returns a _consuming_ enumerator, so it can only be used once.
                int msgCount = 0;
                foreach (var msg in _receiver.RetrieveMessages()) 
                {
                    ++msgCount;
                }
                msgCount.Should().Be(3);
            }
        }


        [Fact]
        public void verify_begin_scope()
        {
            // Prepare listener to receive messages
            var opts = new Ext.UdpLoggerOptions();

            using (_receiver = new UdpReceiver())
            {
                _receiver.Start(opts);

                // Add options to provider before create logger
                using (_provider = new Ext.UdpLoggerProvider(opts)) {
                    _logger = _provider.CreateLogger("test") as Ext.UdpLogger;

                    using (_logger.BeginScope("SCOPE L1: ", null))
                    {
                        _logger.Log(LogLevel.Trace, "some log message");
                        using (_logger.BeginScope("SCOPE L2: ", null))
                        {
                            _logger.Log(LogLevel.Trace, "some log message");
                        }

                        // TODO: confirm scope messages?
                    }

                }
            }
        }


        private LogLevel GetRandomLogLevel(bool excludeNone = true)
        {
            return (LogLevel)_randomizer.Next(0, excludeNone ? _logLevels.Length - 1 : _logLevels.Length);
        }


        #region ITestOutputHelper
        private readonly Xunit.Abstractions.ITestOutputHelper testLog;

        public UdpLoggerTests(Xunit.Abstractions.ITestOutputHelper outputHelper)
        {
            this.testLog = outputHelper;

            _logLevels = Enum.GetValues(typeof(LogLevel));
        }
        #endregion ITestOutputHelper



        private ILoggerProvider _provider;
        private ILogger _logger;
        private UdpReceiver _receiver;
        private readonly object DEFAULT_STATE = "### Default state for testing ###";
        private readonly object NULL_STATE = "[null-state]"; // TODO: use null;
        private readonly Exception DEFAULT_EXCEPTION = new Exception("### Default exception for testing Logging ###");
        private readonly Exception NULL_EXCEPTION = new Exception("[null-exception]"); // TODO: use null;
        private readonly EventId DEFAULT_EVENTID = new EventId();
        private readonly Func<object, Exception, string> setStringFormatter = (state, exception) => $"{state}, {exception}";

        private readonly Array _logLevels;
        private readonly Random _randomizer = new Random();
    }
}
