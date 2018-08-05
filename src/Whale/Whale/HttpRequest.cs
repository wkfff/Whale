/// ---------------------------------------------------------------------------------------------------
/// HTTPRequest.cs
/// 
///     功能：HTTP请求体
///     日期：2018-07-31
///     作者：曾祥极
///     邮箱：tsangciee@gmail.com
///     
/// ---------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;

namespace Whale
{
    public class HTTPRequest
    {
        /// <summary>
        /// 缓冲区最大长度（2M）
        /// </summary>
        private const int MAX_BUFFER_SIZE = 2 * 1024 * 1024;

        /// <summary>
        /// 网络流缓冲区
        /// </summary>
        private byte[] mBuffer = new byte[MAX_BUFFER_SIZE];

        /// <summary>
        /// 网络流
        /// </summary>
        private NetworkStream mStrem { get; set; }

        /// <summary>
        /// 字段集合
        /// </summary>
        public Dictionary<string, string> Parameters { get; private set; }

        /// <summary>
        /// 头部数据
        /// </summary>
        public Dictionary<string, string> Headers { get; private set; }

        /// <summary>
        /// 请求方法
        /// </summary>
        public string HttpMethod { get; private set; }

        /// <summary>
        /// 请求路径
        /// </summary>
        public string URL { get; private set; }

        /// <summary>
        /// 协议版本
        /// </summary>
        public string ProtocolVersion { get; private set; }

        /// <summary>
        /// 请求数据
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="stream">网络流</param>
        public HTTPRequest(NetworkStream stream)
        {
            this.mStrem = stream;
            this.Headers = new Dictionary<string, string>();
            this.Parameters = new Dictionary<string, string>();
            StringBuilder data = new StringBuilder();
            // 读取数据
            int len = 0;
            do
            {
                len = this.mStrem.Read(mBuffer, 0, MAX_BUFFER_SIZE);
                data.Append(Encoding.UTF8.GetString(mBuffer, 0, len));
            }while (this.mStrem.DataAvailable);
            var rows = Regex.Split(data.ToString(), Environment.NewLine);  //得到所有数据行
            // 状态行
            var status = rows[0].Split(' ');
            this.HttpMethod = status[0];
            this.ProtocolVersion = status[2];
            if (this.HttpMethod.ToUpper().Equals("GET") && status[1].Contains("?"))
            {
                // 有参数
                var temp = status[1].Split('?');
                this.URL = temp[0];
                foreach(var p in temp[1].Split('&'))
                {
                    var keyValue = p.Split('=');
                    this.Parameters[keyValue[0].Trim()] = keyValue[1].Trim();
                }
            }
            else
            {
                //无参数
                this.URL = status[1];
            }
            // 请求头
            int start = 1, end = rows.Length;
            for(; start < end; start++)
            {
                if (rows[start].Equals(""))
                {
                    start++;
                    break;
                }
                var keyValue = rows[start].Split(':');
                this.Headers[keyValue[0].Trim()] = keyValue[1].Trim();
            }
            // 请求体
            StringBuilder builder = new StringBuilder();
            for (; start < end; start++)
            {
                builder.AppendLine(rows[start]);
            }
            this.Body = builder.ToString();
        }
    }
}
