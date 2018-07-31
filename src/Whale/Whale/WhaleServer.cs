using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Whale
{
    public class WhaleServer
    {
        /// <summary>
        /// IP
        /// </summary>
        private string mIP { get; set; }

        /// <summary>
        /// 端口
        /// </summary>
        private int mPort { get; set; }

        /// <summary>
        /// 控制层
        /// </summary>
        private List<IController> mControllers { get; set; }

        /// <summary>
        /// TCP监听器
        /// </summary>
        private TcpListener mTCPListener { get; set; }

        /// <summary>
        /// 服务是否正在运行
        /// </summary>
        public bool IsRunning { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="ip">服务IP</param>
        /// <param name="port">服务端口</param>
        /// <param name="controllers">服务业务控制对象</param>
        public WhaleServer(string ip, int port, List<IController> controllers)
        {
            this.mIP = ip;
            this.mPort = port;
            this.mControllers = controllers;
            this.IsRunning = false;
        }

        /// <summary>
        /// 启动服务
        /// </summary>
        public void Start()
        {
            if (this.IsRunning)
            {
                return;
            }
            mTCPListener = new TcpListener(IPAddress.Parse(this.mIP), this.mPort);
            mTCPListener.Start();
            this.IsRunning = true;
            Console.WriteLine(string.Format("service start at {0}:{1}", this.mIP, this.mPort));
            try
            {
                while (this.IsRunning)
                {
                    TcpClient client = this.mTCPListener.AcceptTcpClient();
                    Thread thread = new Thread(() => { ProcessRequest(client); });
                    thread.Start();
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// 停止服务
        /// </summary>
        public void Stop()
        {
            if (!this.IsRunning)
            {
                return;
            }
            this.IsRunning = false;
            this.mTCPListener.Stop();
        }

        /// <summary>
        /// 处理请求
        /// </summary>
        /// <param name="client">从客户端传来的TCP对象</param>
        private void ProcessRequest(TcpClient client)
        {
            Stream stream = client.GetStream();
            StreamReader reader = new StreamReader(stream);
            string[] contents = (reader.ReadLine().Split(' ')[1]).Split('?');
            string url = contents[0];
            Dictionary<string, string> paras = new Dictionary<string, string>();    //参数
            foreach(string p in contents[1].Split('&'))
            {
                string[] kv = p.Split('=');
                paras[kv[0]] = kv[1];
            }
            // 反射路由方法
            foreach(IController c in this.mControllers)
            {
                var methods = c.GetType().GetMethods();
                foreach(var method in methods)
                {
                    object[] objs = method.GetCustomAttributes(typeof(RequestMappingAttribute), true);
                    if(objs.Count() > 0)
                    {
                        RequestMappingAttribute attribute = objs[0] as RequestMappingAttribute;
                        if (url.Equals(attribute.RoutePath))
                        {
                            // 获取参数
                            var methodParas = method.GetParameters();
                            object[] temp = new object[methodParas.Count()];
                            for(int i = 0; i < methodParas.Count(); i++)
                            {
                                temp[i] = paras[methodParas[i].Name];
                            }
                            object response = method.Invoke(c, temp);
                            byte[] bytes = null;
                            if (stream.CanWrite)
                            {
                                // 发送响应头
                                string header = string.Format("HTTP/1.1 200\r\nContent-Type:text/plain\r\n");
                                bytes = Encoding.Default.GetBytes(header.ToString());
                                stream.Write(bytes, 0, bytes.Length);
                                // 发送空行
                                bytes = Encoding.Default.GetBytes("\r\n");
                                stream.Write(bytes, 0, bytes.Length);
                                // 发送正文
                                bytes = Encoding.Default.GetBytes(response.ToString());
                                stream.Write(bytes, 0, bytes.Length);
                            }
                            stream.Close();
                            break;
                        }
                    }
                }
            }
        }
    }
}
