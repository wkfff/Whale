/// ---------------------------------------------------------------------------------------------------
/// WhaleServer.cs
/// 
///     功能：服务类
///     日期：2018-07-31
///     作者：曾祥极
///     邮箱：tsangciee@gmail.com
///     
/// ---------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
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
        public WhaleServer(string ip, int port)
        {
            this.mIP = ip;
            this.mPort = port;
            this.mControllers = new List<IController>();
            this.IsRunning = false;
        }

        /// <summary>
        /// 注册服务
        /// </summary>
        /// <param name="controller">控制层对象</param>
        public void RegisterController(IController controller)
        {
            this.mControllers.Add(controller);
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
            NetworkStream stream = client.GetStream();
            try
            {
                HTTPRequest request = new HTTPRequest(stream);
                // 路由寻址
                foreach (IController c in this.mControllers)
                {
                    var methods = c.GetType().GetMethods(); // 得到控制层对象所有方法
                    foreach (var method in methods)
                    {
                        object[] attributes = method.GetCustomAttributes(typeof(RequestMappingAttribute), true);    // 得到函数的所有RequestMappingAttribute特性
                        if (attributes.Count() > 0)
                        {
                            RequestMappingAttribute attribute = attributes[0] as RequestMappingAttribute;
                            if (request.URL.Equals(attribute.RoutePath))    // 如果函数RequestMappingAttribute路径与Request.URL相同，寻址成功
                            {
                                //var methodParameters = method.GetParameters();   // 获取参数
                                //object[] parameters = new object[methodParameters.Count()];
                                //for (int i = 0; i < methodParameters.Count(); i++)
                                //{
                                //    parameters[i] = request.Parameters[methodParameters[i].Name];
                                //}
                                object response = method.Invoke(c, new object[]{ request});
                                if (stream.CanWrite)
                                {
                                    HTTPResponse httpResponse = new HTTPResponse(stream, "200 OK");
                                    httpResponse.Send(response.ToString());
                                    Thread.CurrentThread.Abort();
                                }
                                break;
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                if (stream.CanWrite)
                {
                    HTTPResponse httpResponse = new HTTPResponse(stream, "200 OK");
                    httpResponse.Send(string.Format("{{'status': 'false', 'msg': '{0}'}}", ex.Message));
                }
            }
        }
    }
}
