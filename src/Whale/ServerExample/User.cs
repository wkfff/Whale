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
        /// <returns></returns>
        [RequestMapping("/user/login")]
        public string Login(HTTPRequest request)
        {
            return string.Format("{{'username': '{0}', 'password': '{1}'}}", request.Parameters["username"], request.Parameters["password"]);
        }
    }
}
