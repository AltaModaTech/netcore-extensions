// Copyright (c) AltaModa Technologies. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.


namespace AMT.Extensions.Logging.IP
{
    using System.Net;

    public class UdpLoggerOptions
    {
        public UdpLoggerOptions(IPEndPoint endPoint)
        {
            _destinationEP = endPoint;
        }

        public UdpLoggerOptions()
        {
            _destinationEP = new IPEndPoint(IPAddress.Loopback, UdpLoggerOptions.DefaultPort);
        }

        private IPEndPoint _destinationEP;
        public static readonly int DefaultPort = 17466;
    }
}