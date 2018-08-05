﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Whale
{
    public enum HTTPMethod
    {
        POST,   // 向指定资源提交数据进行处理请求（例如提交表单或者上传文件）。数据被包含在请求体中。POST请求可能会导致新的资源的建立和/或已有资源的修改。
        GET,    // 请求指定的页面信息，并返回实体主体。
        PUT,    // 从客户端向服务器传送的数据取代指定的文档的内容。
        DELETE  // 请求服务器删除指定的页面。
    }
}
