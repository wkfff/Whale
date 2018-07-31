using System.Collections.Generic;
using Whale;

namespace ServerExample
{
    class Program
    {
        /// <summary>
        /// User控制层
        /// </summary>
        public class User: IController
        {
            /// <summary>
            /// 登录方法
            /// </summary>
            /// <param name="username">用户名</param>
            /// <param name="password">密码</param>
            /// <returns></returns>
            [RequestMapping("/user/login")]
            public string Login(string username, string password)
            {
                return string.Format("[username = {0}, password = {1}]", username, password);
            }
        }
        /// <summary>
        /// 主方法
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            // 注册需要路由的控制层对象
            List<IController> users = new List<IController>();
            users.Add(new User());
            // 生成启动服务
            WhaleServer server = new WhaleServer("127.0.0.1", 8080, users);
            server.Start();
        }
    }
}
