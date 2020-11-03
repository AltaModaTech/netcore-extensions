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
    using System.Threading;

    internal class UdpReceiver
    {
        // private readonly Thread _listernerThread;
        private static int _maxQueuedMessages = 1024;
        private readonly BlockingCollection<string> _messageQueue = new BlockingCollection<string>(_maxQueuedMessages);

        private bool _stopListener = false;
        private UdpClient _client;

        public void Start(Ext.UdpLoggerOptions options)
        {
            _stopListener = false;

            _client = new UdpClient(options.IPEndPoint);
            _client.BeginReceive(new AsyncCallback(ProcessReceived), this);

        }

        public void Stop()
        {
            _stopListener = true;
        }


        public IList<string> ConsumeMessages()
        {
            var list = new List<string>(_messageQueue.Count);

            // await Task.Run(async() =>
            // {

            // });
            // list.AddRange(_messageQueue.GetConsumingEnumerable());

        }

        void ProcessReceived(IAsyncResult ar)
        {
            var udpReceiver = (UdpReceiver) ar.AsyncState;
            var senderIP = new IPEndPoint(0,0);

            while (!_stopListener)
            {
                // Receive Udp data
                byte[] message = udpReceiver._client.EndReceive(ar, ref senderIP);
                // Store it
                _messageQueue.Add(System.Text.Encoding.ASCII.GetString(message));
                // Listen again
                udpReceiver._client.BeginReceive(new AsyncCallback(ProcessReceived), this);
            }

        }


    }
}