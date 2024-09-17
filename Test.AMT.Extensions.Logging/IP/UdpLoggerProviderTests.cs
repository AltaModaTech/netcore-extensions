// Copyright (c) AltaModa Technologies. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Ext = AMT.Extensions.Logging.IP;
using FluentAssertions;
using System;
using System.Diagnostics.CodeAnalysis;
using Xunit;
using AMT.Extensions.Logging.IP;
using Microsoft.Extensions.Logging;
using System.Configuration.Provider;


namespace Test.AMT.Extensions.Logging.IP
{
    [ExcludeFromCodeCoverage]
    public class UdpLoggerProviderTests
    {

        [Fact]
        public void excp_on_null_options()
        {
			Action act = () => new Ext.UdpLoggerProvider(null);
			act.Should().Throw<ArgumentNullException>();
        }


        #region ILoggerProvider tests

        [Fact]
        public void foo()
        {
            var udpProv = new Ext.UdpLoggerProvider(_opts);

            ILoggerProvider prov = udpProv as ILoggerProvider;
            Assert.NotNull(prov);

            ILogger l = prov.CreateLogger("randomCategory");
            Assert.NotNull(l);

            l.LogInformation("using ILogger.LogInformation...");

        }

        #endregion ILoggerProvider tests


        private static readonly UdpLoggerOptions _opts = new UdpLoggerOptions();
    }

}
