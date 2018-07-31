/// ---------------------------------------------------------------------------------------------------
/// RequestParameterAttribute.cs
/// 
///     功能：请求参数特性
///     日期：2018-07-31
///     作者：曾祥极
///     邮箱：tsangciee@gmail.com
///     
/// ---------------------------------------------------------------------------------------------------
using System;

namespace Whale
{
    [AttributeUsage(AttributeTargets.Parameter)]
    class RequestParameterAttribute: Attribute
    {
        /// <summary>
        /// 参数名称
        /// </summary>
        public string ParameterName { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="parameterName">参数</param>
        public RequestParameterAttribute(string parameterName)
        {
            this.ParameterName = parameterName;
        }
    }
}
