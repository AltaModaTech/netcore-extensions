// Copyright (c) AltaModa Technologies. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Concurrent;
using System.Net.Sockets;
using System.Threading;

namespace AMT.Extensions.Logging.IP
{

    public class UdpLoggerProcessor : IDisposable
    {
        private const int _maxQueuedMessages = 1024;

        private readonly BlockingCollection<LogMessageEntry> _messageQueue = new BlockingCollection<LogMessageEntry>(_maxQueuedMessages);
        private readonly Thread _outputThread;
        private readonly UdpLoggerOptions _options;
        private UdpClient _udpSender;
        private bool _shutdown = false;

        public UdpLoggerProcessor(UdpLoggerOptions options)
        {
            if (null == options)  { throw new ArgumentNullException(nameof(options)); }
            _options = options;
            
            _udpSender = new UdpClient(); //options.IPEndPoint);

            // Start message queue processor
            _shutdown = false;
            _outputThread = new Thread(ProcessLogQueue)
            {
                IsBackground = true,
                Name = "Udp logger queue processing thread"
            };
            _outputThread.Start();
        }

        public virtual void EnqueueMessage(LogMessageEntry message)
        {
            if (!_messageQueue.IsAddingCompleted)
            {
                try
                {
                    _messageQueue.Add(message);
                    return;
                }
                catch (InvalidOperationException) { }
            }

            // Adding is completed so just log the message
            WriteMessage(message);
        }

        internal virtual void WriteMessage(LogMessageEntry entry)
        {
            var buff = System.Text.Encoding.ASCII.GetBytes(
                string.Format($"{entry.Message} (sent {DateTime.Now})")
            );

            int sent = _udpSender.Send(buff, buff.Length, _options.IPEndPoint);
        }

        private void ProcessLogQueue()
        {
            while (!_shutdown)
            {
                try
                {
                    foreach (LogMessageEntry message in _messageQueue.GetConsumingEnumerable())
                    {
                        WriteMessage(message);
                    }
                }
                catch /* TODO: review catch & eat */ 
                {
                    try
                    {
                        _messageQueue.CompleteAdding();
                    }
                    catch {  /* TODO: review catch & eat */ }
                }
                // catch (Exception ) { throw; }  // TODO: REMOVE?
            }
        }


        #region IDisposable impl

        public void Dispose()
        {
            _messageQueue.CompleteAdding();
            _shutdown = true;

            // Give the sender time to complete its tasks
            try
            {
                // TODO: use a config value for timeout
                _outputThread.Join(1500); // with timeout in-case Console is locked by user input
            }
            catch (ThreadStateException) { }

            _udpSender.Dispose();
            // Clear any messages
            _messageQueue.Dispose();
        }

        #endregion IDisposable impl
    }
}