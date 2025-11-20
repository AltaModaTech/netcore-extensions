// Copyright (c) AltaModa Technologies. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

// TODO: REMOVE:  just borrowing from https://github.com/dotnet/runtime/blob/6072e4d3a7a2a1493f514cdf4be75a3d56580e84/src/libraries/Common/src/Extensions/Logging/NullScope.cs
//  due to it being internal access there.

using System;
using System.Diagnostics.CodeAnalysis;

namespace AMT.Extensions.Logging.IP
{
    /// <summary>
    /// An empty scope without any logic
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class NullScope : IDisposable
    {
        public static NullScope Instance { get; } = new NullScope();

        private NullScope()
        {
        }

        /// <inheritdoc />
        public void Dispose()
        {
        }
    }
}