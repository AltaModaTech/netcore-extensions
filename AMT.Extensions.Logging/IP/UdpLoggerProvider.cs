// Copyright (c) AltaModa Technologies. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.Extensions.Logging;


namespace AMT.Extensions.Logging.IP
{
    /// <summary>
    /// An <see cref="ILoggerProvider"/> the sends logs over UDP.
    /// </summary>
    [ProviderAlias("Udp")]
    public class UdpLoggerProvider : ILoggerProvider, ISupportExternalScope
    {

        #region ILoggerProvider impl

        public ILogger CreateLogger(string category)
        {
            // TODO: gather options from config
            return new UdpLogger(category, new UdpLoggerProcessor(new UdpLoggerOptions()));
        }

        #endregion ILoggerProvider impl



        #region ISupportExternalScope impl

        public void SetScopeProvider(IExternalScopeProvider scopeProvider)
        {
            _scopeProvider = scopeProvider;
        }

        #endregion ISupportExternalScope impl



        #region IDisposable impl

        /// <inheritdoc />
        public void Dispose()
        {
        }

        #endregion IDisposable impl


        private IExternalScopeProvider _scopeProvider = NullExternalScopeProvider.Instance;

    }
}
