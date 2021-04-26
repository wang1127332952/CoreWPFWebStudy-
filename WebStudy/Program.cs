using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace WebStudy
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args).ConfigureLogging((context, log) =>
            {
                //¹ýÂËÈÕÖ¾
                log.AddFilter("System", LogLevel.Information);

            }).ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseIIS();
                webBuilder.UseStartup<Startup>();
            });
    }
}
