using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace WebStudy.Middleware
{
    public class Test_Middleware
    {
        private readonly RequestDelegate _next;

        public Test_Middleware(RequestDelegate next) 
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext) 
        {
            //在这里编写中间件业务
            //这里是http请求部分
            await _next(httpContext);
            //这是http响应部分
        }

    }
}
