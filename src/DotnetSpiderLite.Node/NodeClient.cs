using DotnetSpiderLite.Downloader;
using DotnetSpiderLite.Node.Downloader;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;

namespace DotnetSpiderLite.Node
{
    public class NodeClient : IDisposable
    {
        private IDownloader _downloader = new DefaultHttpClientDownloader();
        private NodeClientStatus _status = NodeClientStatus.Exited;

        private HttpClient _hubHttpClient = new HttpClient();

        private string _hubAliveEndPoint;
        private string _hubDataEndPoint;

        public IDownloader Downloader => _downloader;

        public NodeClientStatus Status => _status;


        public string NodeId { get; } = new Guid().ToString("N");


        public NodeClient(string hubAddress)
        {
            _hubAliveEndPoint = string.Format("node/live", hubAddress);
            _hubDataEndPoint = string.Format("node/data", hubAddress);

        }


        public NodeClient SetDownloader(IDownloader downloader)
        {
            _downloader = downloader;
            return this;
        }


        public void Start()
        {
            if (_status == NodeClientStatus.Running)
            {
                return;
            }

            _status = NodeClientStatus.Running;

            while (true)
            {
                if (_status == NodeClientStatus.Running)
                {
                    DoWork();
                }

                Thread.Sleep(100);
            }

        }

        public void Stop()
        {
            _status = NodeClientStatus.Exited;
        }



        private void DoWork()
        {
            try
            {
                var response = _hubHttpClient.GetStringAsync(_hubAliveEndPoint).Result;

                var command = Newtonsoft.Json.JsonConvert.DeserializeObject<NodeClientCommand>(response);

                if (command.Command == 10)
                {
                    var request = ConventToRequest(command.CommandData);
                    HandleRequest(request);
                }
                else if (command.Command == 1)
                {
                    _status = NodeClientStatus.Exited;
                }

            }
            catch (Exception)
            {

            }

        }


        private void HandleRequest(Request request)
        {
            var response = _downloader.DownloadAsync(request).Result;

            if (response.Body == null)
            {
                return;
            }

            var data = new
            {
                type = 1, // 1:response,
                data = Convert.ToBase64String(response.Body),
            };

            _hubHttpClient.PostAsync(_hubDataEndPoint, new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(data)));
        }



        static Request ConventToRequest(string requestJson)
        {
            var d = new
            {
                Referer = "",
                KeepAlive = false,
                AcceptLanguage = "",
                AcceptEncoding = "",
                Accept = "",
                UserAgent = "",
                ContentType = "",
                Encoding = "",
                Method = "",
                Url = "",
                Body = default(byte[]),
                Headers = default(Dictionary<string, string>),
                Extra = default(Dictionary<string, string>),
            };

            var data = Newtonsoft.Json.JsonConvert.DeserializeAnonymousType(requestJson, d);

            var request = new Request(new Uri(data.Url))
            {
                Accept = data.Accept,
                KeepAlive = data.KeepAlive,
                AcceptLanguage = data.AcceptLanguage,
                AcceptEncoding = data.AcceptEncoding,
                UserAgent = data.UserAgent,
                ContentType = data.ContentType,
                Encoding = Encoding.GetEncoding(data.Encoding),
                Method = data.Method,
                Body = data.Body,
                Referer = data.Referer,
            };

            return request;
        }

        public void Dispose()
        {
        }

    }
}
