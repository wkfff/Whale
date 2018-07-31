/// ---------------------------------------------------------------------------------------------------
/// RequestMappingAttribute.cs
/// 
///     功能：负责对函数的地址特性进行路由
///     日期：2018-07-31
///     作者：曾祥极
///     邮箱：tsangciee@gmail.com
///     
/// ---------------------------------------------------------------------------------------------------
using System;

namespace Whale
{
    [AttributeUsage(AttributeTargets.Method)]
    public class RequestMappingAttribute: Attribute
    {
        /// <summary>
        /// 路由地址
        /// </summary>
        public string RoutePath { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="routePath"></param>
        public RequestMappingAttribute(string routePath)
        {
            this.RoutePath = routePath;
        }
    }
}
