using Ext = AMT.Extensions.Logging.IP;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using System;
using Xunit;
using Xunit.Abstractions;


namespace Test.AMT.Extensions.Logging.IP
{
    public class UdpLoggerTests
    {

        [Fact]
        public void verify_few_log_messages()
        {
            // Prepare listener to receive messages
            var opts = new Ext.UdpLoggerOptions();
            testLog.WriteLine($"UDP options: {opts.IPEndPoint.ToString()}");
            var r = new UdpReceiver();
            r.Start(opts);

            // Add options to provider before create logger
            _provider = new Ext.UdpLoggerProvider(opts);
            var l = _provider.CreateLogger("test") as Ext.UdpLogger;

            // Log some messages
            l.Log(LogLevel.Debug, DEFAULT_EVENTID, DEFAULT_STATE, DEFAULT_EXCEPTION, setStringFormatter);
            l.Log(LogLevel.Debug, DEFAULT_EVENTID, NULL_STATE, DEFAULT_EXCEPTION, setStringFormatter);
            l.Log(LogLevel.Debug, DEFAULT_EVENTID, DEFAULT_STATE, NULL_EXCEPTION, setStringFormatter);
            // LogLevel None results is no log
            l.Log(LogLevel.None, DEFAULT_EVENTID, DEFAULT_STATE, NULL_EXCEPTION, setStringFormatter);

            // Brief wait & stop listener
            System.Threading.Thread.Sleep(100);
            r.Stop();


            // NOTE: RetrieveMessages returns a _consuming_ enumerator, so it can only be used once.
            int msgCount = 0;
            foreach (var msg in r.RetrieveMessages()) 
            {
                ++msgCount;
            }
            msgCount.Should().Be(3);
        }


        #region ITestOutputHelper
        private readonly ITestOutputHelper testLog;

        public UdpLoggerTests(ITestOutputHelper outputHelper)
        {
            this.testLog = outputHelper;
        }
        #endregion ITestOutputHelper



        private ILoggerProvider _provider;
        private readonly object DEFAULT_STATE = "### Default state for testing ###";
        private readonly object NULL_STATE = "[null-state]"; // TODO: use null;
        private readonly Exception DEFAULT_EXCEPTION = new Exception("### Default exception for testing Logging ###");
        private readonly Exception NULL_EXCEPTION = new Exception("[null-exception]"); // TODO: use null;
        private readonly EventId DEFAULT_EVENTID = new EventId();
        private readonly Func<object, Exception, string> setStringFormatter = (state, exception) => $"{state}, {exception}";
        
    }
}
