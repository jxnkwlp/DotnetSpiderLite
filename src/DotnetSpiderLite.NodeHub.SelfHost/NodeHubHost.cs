﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;

namespace DotnetSpiderLite.NodeHub.SelfHost
{
    public class NodeHubHost : IDisposable
    {
        public const int DEFAULTPORT = 10030;

        private int _httpPort = DEFAULTPORT;
        private HttpListener _listener = new HttpListener();

        private Thread _thread;

        private NodeManager _nodeManager;


        public NodeHubHost(int port)
        {
            this._httpPort = port;

            Initialize();
        }

        public NodeHubHost() : this(DEFAULTPORT)
        {
        }


        private void Initialize()
        {
            _thread = new Thread(Listen);
            _thread.Start();
        }

        private void Listen()
        {
            _listener = new HttpListener();
            _listener.Prefixes.Add($"http://*:{_httpPort}/");
            _listener.Start();

            while (true)
            {
                try
                {
                    HttpListenerContext context = _listener.GetContext();
                    Process(context);
                }
                catch (Exception ex)
                {
                }
            }
        }

        private void Process(HttpListenerContext context)
        {
            var path = context.Request.Url.AbsolutePath;

            if (!path.StartsWith("/api/node/"))
            {
                context.Response.StatusCode = 404;
                context.Response.OutputStream.Close();
                return;
            }



            var paths = path.Split('/');

            if (paths.Length != 4)
            {
                context.Response.StatusCode = 404;
                context.Response.OutputStream.Close();
                return;
            }


            string nodeId = paths[2];
            var r = paths[3];

            if (r.Equals("ping", StringComparison.InvariantCultureIgnoreCase))
            {
                ProcessPing(context);
            }
            else if (r.Equals("task", StringComparison.InvariantCultureIgnoreCase))
            {
                ProcessTask(context);
            }
            else if (r.Equals("control", StringComparison.InvariantCultureIgnoreCase))
            {
                ProcessControl(context);
            }


        }


        private void ProcessPing(HttpListenerContext context)
        {
            _nodeManager.AddOrUpdateNode(new Node());

        }

        private void ProcessTask(HttpListenerContext context)
        {
            context.Response.StatusCode = 200;

        }

        private void ProcessControl(HttpListenerContext context)
        {
            context.Response.StatusCode = 200;

        }

        public void Dispose()
        {
            _listener?.Stop();
        }
    }
}
