using DotnetSpiderLite.Downloader;
using DotnetSpiderLite.Node.Downloader;
using DotnetSpiderLite.Node.Model;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DotnetSpiderLite.Node
{
    public class NodeClient : IDisposable
    {
        private IDownloader _downloader = new DefaultHttpClientDownloader();
        private NodeClientStatus _status = NodeClientStatus.Exited;

        private bool _handleRequested = false;
        private HttpClient _hubHttpClient = new HttpClient();

        private string _hubHeartbeatEndPoint = "{0}/api/node/{1}/ping"; // post
        private string _hubTaskEndPoint = "{0}/api/node/{1}/task"; // get 
        // private string _hubDataEndPoint = "{0}/api/node/{1}/pushdata"; // post 
        private string _hubControlEndPoint = "{0}/api/node/{1}/control"; // get 

        public IDownloader Downloader => _downloader;

        public NodeClientStatus Status => _status;


        public string NodeId { get; } = new Guid().ToString("N");


        public NodeClient(string hubAddress)
        {
            _hubHeartbeatEndPoint = string.Format(_hubHeartbeatEndPoint, hubAddress, this.NodeId);
            _hubTaskEndPoint = string.Format(_hubTaskEndPoint, hubAddress, this.NodeId);
            // _hubDataEndPoint = string.Format(_hubDataEndPoint, hubAddress, this.NodeId);
            _hubControlEndPoint = string.Format(_hubControlEndPoint, hubAddress, this.NodeId);

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

                Thread.Sleep(1000); // 1s
            }

        }

        public void Stop()
        {
            _status = NodeClientStatus.Exited;
        }



        private void DoWork()
        {
            SendHeartbeat();
            PullControl();
            PullTask();
        }

        /// <summary>
        ///  发送心跳
        /// </summary>
        private void SendHeartbeat()
        {
            var data = new HeartbeatModel()
            {
                // NodeId = this.NodeId,
                Status = this.Status.ToString(),
            };

            try
            {
                var response = _hubHttpClient.PostAsync(_hubHeartbeatEndPoint, new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(data))).Result;

                response.EnsureSuccessStatusCode();
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        ///  拉取任务
        /// </summary>
        private void PullTask()
        {
            if (_handleRequested)
                return;

            _handleRequested = true;

            try
            {
                var response = _hubHttpClient.GetAsync(_hubTaskEndPoint).Result;
                response.EnsureSuccessStatusCode();

                var json = response.Content.ReadAsStringAsync().Result;

                var task = Newtonsoft.Json.JsonConvert.DeserializeObject<TaskPullModel>(json);

                if (task != null)
                {
                    var request = ConventToRequest(task.RequestJson);

                    if (!string.IsNullOrEmpty(task.CookieHeader))
                    {
                        DownloaderCookieContainer.Instance.SetCookies(request.Uri, task.CookieHeader);
                    }

                    Task.Run(() => HandleNewRequest(request));
                }


            }
            catch (Exception)
            {
            }

            _handleRequested = false;

        }

        private void HandleNewRequest(Request request)
        {
            var response = _downloader.DownloadAsync(request).Result;

            var data = new TaskPushModel()
            {
                ResponseUrl = response.ResponseUri.ToString(),
                Response = response.Body,
            };

            _hubHttpClient.PostAsync(_hubTaskEndPoint, new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(data)));
        }

        /// <summary>
        ///  获取控制指令
        /// </summary>
        private void PullControl()
        {
            try
            {
                var response = _hubHttpClient.GetAsync(_hubTaskEndPoint).Result;
                response.EnsureSuccessStatusCode();

                var json = response.Content.ReadAsStringAsync().Result;

                var data = Newtonsoft.Json.JsonConvert.DeserializeObject<ControlModel>(json);

                if (data != null)
                {
                    if (data.Command == 1)
                    {
                        this._status = NodeClientStatus.Exiting;
                    }

                    // TODO 其他指令
                    // 
                }

            }
            catch (Exception)
            {
            }
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
