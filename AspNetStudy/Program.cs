using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AspNetStudy
{
    /// <summary>
    /// EF6.0 
    /// 1.EFCore3.0 两种方式应用
    /// 2.EF 整合EFcore完成数据查询
    /// 3.分层封装 autofac 完成依赖倒置,搭建框架
    /// EF6 4种模式 
    /// CodeFirst 
    /// DbFirst 
    /// ModelFirst
    /// CodeFirst from DB
    /// </summary>
    public class Program
    {
        public static void Main(string[] args)
        {
            //string i = null;
            //if (i?.Length > 0)
            //{
            //    int f = 1;
            //}
            //else 
            //{
            //    int j = 3;
            //}

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
