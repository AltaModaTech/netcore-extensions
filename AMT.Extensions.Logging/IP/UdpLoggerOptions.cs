// Copyright (c) AltaModa Technologies. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.


namespace AMT.Extensions.Logging.IP
{
    using System;
    using System.Net;

    public class UdpLoggerOptions
    {
        public IPEndPoint IPEndPoint { get; private set; }
        public UdpLoggerOptions(IPEndPoint endPoint)
        {
            if (null == endPoint)  { throw new ArgumentNullException(nameof(endPoint)); }
            this.IPEndPoint = endPoint;
        }

        public UdpLoggerOptions()
        {
            this.IPEndPoint = new IPEndPoint(DefaultAddress, DefaultPort);
        }

        public static readonly IPAddress DefaultAddress = IPAddress.Loopback;
        public static readonly int DefaultPort = 17466;
    }
}