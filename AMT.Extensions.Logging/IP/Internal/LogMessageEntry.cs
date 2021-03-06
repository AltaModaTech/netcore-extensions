// Copyright (c) AltaModa Technologies. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.Extensions.Logging;


namespace AMT.Extensions.Logging.IP
{

    public struct LogMessageEntry
    {
        public EventId EventId;
        public LogLevel LogLevel;
        public string Message;

    }
}