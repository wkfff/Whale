/// ---------------------------------------------------------------------------------------------------
/// HTTPResponse.cs
/// 
///     功能：HTTP响应体
///     日期：2018-07-31
///     作者：曾祥极
///     邮箱：tsangciee@gmail.com
///     
/// ---------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace Whale
{
    public class HTTPResponse
    {
        /// <summary>
        /// 缓冲区最大长度（2M）
        /// </summary>
        private const int MAX_BUFFER_SIZE = 2 * 1024 * 1024;

        /// <summary>
        /// 网络流缓冲区
        /// </summary>
        private byte[] mBuffer;

        /// <summary>
        /// 网络流
        /// </summary>
        private NetworkStream mStrem { get; set; }

        /// <summary>
        /// 状态码
        /// </summary>
        public string StatusCode { get; private set; }
        
        /// <summary>
        /// 状态行
        /// </summary>
        public string StatusLine { get; set; }

        /// <summary>
        /// 协议版本
        /// </summary>
        public string ProtocolVersion { get; private set; }

        /// <summary>
        /// 响应头
        /// </summary>
        public Dictionary<string, string> Headers { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="stream">网络流</param>
        /// <param name="statusCode">状态码（成功请求200 OK）</param>
        public HTTPResponse(NetworkStream stream, string statusCode)
        {
            this.mStrem = stream;
            this.Headers = new Dictionary<string, string>();
            this.StatusCode = statusCode;
            this.ProtocolVersion = "HTTP/1.1";
            this.StatusLine = string.Format("{0} {1}", this.ProtocolVersion, this.StatusCode);
            // 响应头
            this.Headers["Date"] = DateTime.Now.ToShortDateString();
            this.Headers["Server"] = "Whale 0.0.1";
            this.Headers["Content-Type"] = "text/plain";
        }

        /// <summary>
        /// 响应客户端数据
        /// </summary>
        /// <param name="content">消息正文</param>
        public void Send(string content)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine(this.StatusLine);
            foreach(var str in this.Headers.Keys)
            {
                builder.AppendLine(string.Format("{0}: {1}", str, this.Headers[str]));
            }
            builder.AppendLine("");
            builder.Append(content);
            this.mBuffer = Encoding.UTF8.GetBytes(builder.ToString());
            this.mStrem.Write(this.mBuffer, 0, this.mBuffer.Length);
            this.mStrem.Close();
        }
    }
}
