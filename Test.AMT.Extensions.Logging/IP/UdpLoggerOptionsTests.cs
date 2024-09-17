// Copyright (c) AltaModa Technologies. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Ext = AMT.Extensions.Logging.IP;
using FluentAssertions;
using System;
using System.Diagnostics.CodeAnalysis;
using Xunit;


namespace Test.AMT.Extensions.Logging.IP
{
    [ExcludeFromCodeCoverage]
    public class UdpLoggerOptionsTests
    {

        [Fact]
        public void excp_on_null_IP_endpoint()
        {
			Action act = () => new Ext.UdpLoggerOptions(null);
			act.Should().Throw<ArgumentNullException>();
        }

    }

}
