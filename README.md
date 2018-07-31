# <img src="img/whale.png" width="30" height = "30"></img> Whale
## 介绍
##### Whale是一个CSharp编写、restful风格、并且基于http协议的服务软件，它可以帮助你快速开发一些小型服务，目前支持：
    * Http请求
    * 路由方法
## 快速开始
#### 1 安装
##### 进入version目录下，选择最新版本的`whale.dll`下载到本地，然后再项目中引用dll即可。
#### 2 编写业务控制层类User
##### 项目中所有需要进行自动路由寻址方法所在的类均需要通过继承`Whale.IController`接口，只有这样路由寻址才能方便的找到对应的方法。
```
using Whale;

namespace ServerExample
{
    /// <summary>
    /// User控制层
    /// </summary>
    public class User : IController
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
}

```
#### 3 开启服务
```
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

```
#### 4 访问测试
##### 访问<a href="http://127.0.0.1:8080/user/login?username=1&password=1">http://127.0.0.1:8080/user/login?username=1&password=1</a>即可得到如下结果：
```
[username = 1, password = 1]
```