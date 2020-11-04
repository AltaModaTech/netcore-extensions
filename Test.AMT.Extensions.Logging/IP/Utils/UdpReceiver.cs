// Copyright (c) AltaModa Technologies. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Ext = AMT.Extensions.Logging.IP;
using System.Net.Sockets;


namespace Test.AMT.Extensions.Logging.IP
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Net;

    internal class UdpReceiver
    {
        private static int _maxQueuedMessages = 1024;
        private readonly BlockingCollection<string> _messageQueue = new BlockingCollection<string>(_maxQueuedMessages);

        private bool _stopListener = false;
        private UdpClient _client;

        public void Start(Ext.UdpLoggerOptions options)
        {
            _stopListener = false;

            _client = new UdpClient(options.IPEndPoint);
            var ar = _client.BeginReceive(new AsyncCallback(ProcessReceived), this);
        }

        public void Stop()
        {
            _stopListener = true;
            _messageQueue.CompleteAdding();
        }


        public IEnumerable<string> RetrieveMessages()
        {
            return _messageQueue.GetConsumingEnumerable();
        }


        void ProcessReceived(IAsyncResult ar)
        {
            var udpReceiver = (UdpReceiver) ar.AsyncState;
            var senderIP = new IPEndPoint(0,0);

            while (!_stopListener)
            {
                var r = udpReceiver._client.ReceiveAsync().Result;

                if (r.Buffer.Length > 0)
                {
                    string msg = System.Text.Encoding.ASCII.GetString(r.Buffer);
                    udpReceiver._messageQueue.Add(msg);
                }
            }

            // TODO: need to call EndReceive here?
        }

    }
}