using Ext = AMT.Extensions.Logging.IP;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using Xunit;


namespace IPTests
{
    public class UdpLoggerTests
    {

        [Fact]
        public void Test2()
        {
            var l = _provider.CreateLogger("test");

            l.Should().NotBe(null);

            l.Log(LogLevel.Debug, DEFAULT_EVENTID, DEFAULT_STATE, DEFAULT_EXCEPTION, setStringFormatter);
            l.Log(LogLevel.Debug, DEFAULT_EVENTID, NULL_STATE, DEFAULT_EXCEPTION, setStringFormatter);
            l.Log(LogLevel.Debug, DEFAULT_EVENTID, DEFAULT_STATE, NULL_EXCEPTION, setStringFormatter);
        }



        private ILoggerProvider _provider = new Ext.UdpLoggerProvider();
        private readonly object DEFAULT_STATE = "### Default state for testing ###";
        private readonly object NULL_STATE = null;
        private readonly Exception DEFAULT_EXCEPTION = new Exception("### Default exception for testing Logging ###");
        private readonly Exception NULL_EXCEPTION = null;
        private readonly EventId DEFAULT_EVENTID = new EventId();
        private readonly Func<object, Exception, string> setStringFormatter = (state, exception) => $"{state}, {exception}";
        
    }
}
