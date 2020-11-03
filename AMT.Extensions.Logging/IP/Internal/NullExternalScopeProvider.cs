// Copyright (c) AltaModa Technologies. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

// TODO: REMOVE:  just borrowing from https://github.com/dotnet/runtime/blob/6072e4d3a7a2a1493f514cdf4be75a3d56580e84/src/libraries/Common/src/Extensions/Logging/NullExternalScopeProvider.cs
//  due to it being internal access there.

using Microsoft.Extensions.Logging;
using System;

namespace AMT.Extensions.Logging.IP
{
    /// <summary>
    /// Scope provider that does nothing.
    /// </summary>
    internal class NullExternalScopeProvider : IExternalScopeProvider
    {
        private NullExternalScopeProvider()
        {
        }

        /// <summary>
        /// Returns a cached instance of <see cref="NullExternalScopeProvider"/>.
        /// </summary>
        public static IExternalScopeProvider Instance { get; } = new NullExternalScopeProvider();



        #region IExternalScopeProvider impl

        /// <inheritdoc />
        void IExternalScopeProvider.ForEachScope<TState>(Action<object, TState> callback, TState state)
        {
        }

        /// <inheritdoc />
        IDisposable IExternalScopeProvider.Push(object state)
        {
            return NullScope.Instance;
        }

        #endregion IExternalScopeProvider impl
    }
}