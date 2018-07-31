using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Whale;

namespace ServerExample
{
    class Program
    {
        public class User: IController
        {
            
            [RequestMapping("/user/login")]
            public string Test(string username, string password)
            {
                return string.Format("[username = {0}, password = {1}]", username, password);
            }
        }
        static void Main(string[] args)
        {
            List<IController> users = new List<IController>();
            users.Add(new User());
            WhaleServer server = new WhaleServer("127.0.0.1", 8080, users);
            server.Start();
        }
    }
}
