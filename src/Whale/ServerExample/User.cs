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
