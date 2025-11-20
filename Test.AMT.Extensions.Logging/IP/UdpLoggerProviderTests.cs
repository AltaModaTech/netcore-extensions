// Copyright (c) AltaModa Technologies. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Ext = AMT.Extensions.Logging.IP;
using FluentAssertions;
using System;
using System.Diagnostics.CodeAnalysis;
using Xunit;
using AMT.Extensions.Logging.IP;
using Microsoft.Extensions.Logging;


namespace Test.AMT.Extensions.Logging.IP
{
    [ExcludeFromCodeCoverage]
    public class UdpLoggerProviderTests
    {

        [Fact]
        public void excp_on_null_category()
        {
            var provider = new Ext.UdpLoggerProvider(_opts);
			Action act = () => provider.CreateLogger(null);

			act.Should().Throw<ArgumentNullException>();
        }


        [Fact]
        public void excp_on_null_options()
        {
			Action act = () => new Ext.UdpLoggerProvider(null);
			act.Should().Throw<ArgumentNullException>();
        }


        #region ILoggerProvider tests

        [Fact]
        public void can_log_to_category()
        {
            using (var udpProv = new Ext.UdpLoggerProvider(_opts))
            {
                ILoggerProvider prov = udpProv as ILoggerProvider;
                Assert.NotNull(prov);

                ILogger l = prov.CreateLogger("randomCategory");
                Assert.NotNull(l);

                l.LogInformation("using ILogger.LogInformation...");
            }
        }

        #endregion ILoggerProvider tests


        #region ISupportExternalScope tests

        [Fact]
        public void can_set_scope_provider()
        {
            var testEsps = new IExternalScopeProvider[] { 
                null,
                NullExternalScopeProvider.Instance
            };
            // TODO: consider disabling null
            foreach (IExternalScopeProvider esp in testEsps ) {
                using (var udpProv = new Ext.UdpLoggerProvider(_opts))
                {
                    udpProv.SetScopeProvider(esp);

                    ILogger l = udpProv.CreateLogger("randomCategory");
                    Assert.NotNull(l);

                    l.LogInformation("using ILogger.LogInformation...");
                }
            }
        }

        #endregion ISupportExternalScope tests
        


        private static readonly UdpLoggerOptions _opts = new UdpLoggerOptions();
    }

}
