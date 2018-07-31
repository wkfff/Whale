# <img src="img/whale.png" width="30" height = "30"></img> Whale
## 介绍
##### Whale是一个CSharp编写、restful风格、并且基于http协议的服务软件，它可以帮助你快速开发一些小型服务，目前支持：
* 路由方法
## 快速开始
```
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
```