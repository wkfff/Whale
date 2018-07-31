using Whale;

namespace ServerExample
{
    class Program
    {
        
        /// <summary>
        /// 主方法
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            WhaleServer server = new WhaleServer("127.0.0.1", 8080);
            server.RegisterController(new User());  // 注册服务
            server.Start(); // 启动服务
        }
    }
}
