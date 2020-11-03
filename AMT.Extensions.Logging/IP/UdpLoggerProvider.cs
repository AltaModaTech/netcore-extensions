// Copyright (c) AltaModa Technologies. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;


namespace AMT.Extensions.Logging.IP
{
    /// <summary>
    /// An <see cref="ILoggerProvider"/> the sends logs over UDP.
    /// </summary>
    [ProviderAlias("Udp")]
    public class UdpLoggerProvider : ILoggerProvider //, ISupportExternalScope
    {
        public ILogger CreateLogger(string category)
        {
            return new UdpLogger(category);
        }

        #region IDisposable impl

        /// <inheritdoc />
        public void Dispose()
        {
        }

        #endregion IDisposable impl

    }

}
