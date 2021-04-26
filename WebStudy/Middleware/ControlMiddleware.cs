using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebStudy.Middleware
{
    public static class ControlMiddleware
    {
        public static IApplicationBuilder UseTest(this IApplicationBuilder app) 
        {
            return app.UseMiddleware<Test_Middleware>();
        }
    }
}
