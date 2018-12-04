using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;

namespace DotnetSpiderLite.ProxyPools
{
    public class DefaultProxyValidator : IProxyValidator
    {
        public bool IsAvailable(WebProxy proxy)
        {
            Uri targetUri = new Uri("https://www.baidu.com/");

            var result = Validate(proxy, targetUri, TimeSpan.FromSeconds(5));

            return result == HttpStatusCode.OK;
        }


        /// <summary>
        /// 使用http tunnel检测代理状态
        /// </summary>
        /// <param name="webProxy">web代理</param>      
        /// <param name="targetAddress">目标地址，可以是http或https</param>
        /// <param name="timeout">发送或等待数据的超时时间</param>
        /// <exception cref="ArgumentNullException"></exception>    
        /// <returns></returns>
        public static HttpStatusCode Validate(WebProxy webProxy, Uri targetAddress, TimeSpan? timeout = null)
        {
            if (webProxy == null)
            {
                throw new ArgumentNullException(nameof(webProxy));
            }

            string userName = null;
            string password = null;

            if (webProxy.Credentials != null)
            {
                if (webProxy.Credentials is NetworkCredential networkCredential)
                {
                    userName = networkCredential.UserName;
                    password = networkCredential.Password;
                }
            }

            var address = webProxy.Address;
            Socket socket = null;
            try
            {
                var ascii = Encoding.GetEncoding("ASCII");
                var host = Dns.GetHostEntry(address.Host);
                socket = new Socket(host.AddressList[0].AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                if (timeout.HasValue)
                {
                    socket.SendTimeout = (int)timeout.Value.TotalMilliseconds;
                    socket.ReceiveTimeout = (int)timeout.Value.TotalMilliseconds;
                }
                socket.Connect(new IPEndPoint(host.AddressList[0], address.Port));

                var request = ToTunnelRequestString(targetAddress);
                var sendBuffer = ascii.GetBytes(request);
                socket.Send(sendBuffer);

                var recvBuffer = new byte[150];
                var length = socket.Receive(recvBuffer);

                var response = ascii.GetString(recvBuffer, 0, length);
                var statusCode = int.Parse(Regex.Match(response, "(?<=HTTP/1.1 )\\d+", RegexOptions.IgnoreCase).Value);
                return (HttpStatusCode)statusCode;
            }
            catch (Exception)
            {
                return HttpStatusCode.ServiceUnavailable;
            }
            finally
            {
                socket?.Dispose();
            }
        }

        static string ToTunnelRequestString(Uri targetAddress, string username = null, string password = null)
        {
            if (targetAddress == null)
            {
                throw new ArgumentNullException(nameof(targetAddress));
            }

            const string crlf = "\r\n";
            var builder = new StringBuilder()
                .Append($"CONNECT {targetAddress.Host}:{targetAddress.Port} HTTP/1.1{crlf}")
                .Append($"Host: {targetAddress.Host}:{targetAddress.Port}{crlf}")
                .Append($"Accept: */*{crlf}")
                .Append($"Content-Type: text/html{crlf}")
                .Append($"Proxy-Connection: Keep-Alive{crlf}")
                .Append($"Content-length: 0{crlf}");

            if (username != null && password != null)
            {
                var bytes = Encoding.ASCII.GetBytes($"{username}:{password}");
                var base64 = Convert.ToBase64String(bytes);
                builder.AppendLine($"Proxy-Authorization: Basic {base64}{crlf}");
            }
            return builder.Append(crlf).ToString();
        }

    }
}
