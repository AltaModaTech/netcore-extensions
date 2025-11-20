// Copyright (c) AltaModa Technologies. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Ext = AMT.Extensions.Logging.IP;
using FluentAssertions;
using System;
using System.Diagnostics.CodeAnalysis;
using Xunit;
using AMT.Extensions.Logging.IP;


namespace Test.AMT.Extensions.Logging.IP
{
    [ExcludeFromCodeCoverage]
    public class UdpLoggerProcessorTests
    {

       [Fact]
        public void excp_on_null_options()
        {
            Action act = () => new Ext.UdpLoggerProcessor(null);
            act.Should().Throw<ArgumentNullException>();
        }


        #region IDisposable tests

        [Fact]
        public void verify_disposable()
        {
            using (var proc = new Ext.UdpLoggerProcessor(_opts))
            {
                var udpProv = new Ext.UdpLoggerProvider(_opts);
            }

        }

        #endregion IDisposable tests


        private static readonly UdpLoggerOptions _opts = new UdpLoggerOptions();
    }

}
