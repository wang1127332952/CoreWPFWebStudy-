using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using WebStudy.Extension;
using WebStudy.Middleware;

namespace WebStudy
{
    /// <summary>
    /// 配置web应用所需的服务和中间件
    /// </summary>
    public class Startup
    {
        private readonly IConfiguration _configuration;
        public Startup(IConfiguration configuration) 
        {
            _configuration = configuration;
        }


        /// <summary>
        /// 注册服务
        /// </summary>
        /// <param name="services"></param>
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //注册配置选项的服务
            services.Configure<AppSetting>(_configuration);
            //服务容器 
            //Ioc(控制反转)容器：注册类型  请求实例
            //需要“什么”就去拿“什么” 正转  需要“什么”被动获得 反转  “什么”是依赖（把“什么”给你）
            //依赖注入:一系列工具和手段 最终目的是松耦合 可维护 可测试的代码和程序。（）
            //把有依赖关系的类放入IOC容器中，然后解析出这个类的实例

            //IOC容器 反转依赖和接口方式 直接操控的对象控制权给了第三方通过第三方实现 对象组件的转给和管理

            //一个升级版的工厂 还配送物流
            //IOC容器哪里来   框架提供  映射依赖 管理对象的创建和生存期

            //IOC容器（登记处）本身就是对象 注册类型 功能解析 某个类所依赖的对象

            //依赖注入框架 Unity Autofac Ninject 都是.NET的

            //添加控制器和API相关功能的支持，但不支持试图和页面
            //不关心生命周期 
            services.AddControllers();
            //添加控制器和API相关功能的支持,支持试图和页面  ASP.NET CORE 3.X 默认模板
            services.AddControllersWithViews();
            //添加对Razor Page和最小控制器的支持
            services.AddRazorPages();
            //ASP.NET CORE 2.x
            services.AddMvc();
            //跨域
            services.AddCors();
            //内置服务 也可以添加 第三方服务(EF Swagger) 

            //注册自定义服务
            //服务生命周期   类型生命周期

            //注册自定义服务的时间 必须选择生命周期

            //瞬时 每次从服务容器里进行请求实例时 都会创建一个新的实例
            //作用域 线程单例 在同一个线程（请求）里 值实例化一次
            //单例 全局单例 只会创建一次 每一次都使用相同的实例
            // services.AddTransient()
            // services.AddScoped()
            // services.AddSingleton()
            //注册多个 最后一个生效
            //服务 到底怎么用 用哪个生命周期
            //自定义扩展
            //services.AddMessages();
            //自定义服务
            //services.AddMessages(builder=> builder.UseEmail());

        }

        

        /// <summary>
        /// 配置中间件,中间件组成管道
        /// 必须的 中间件就是处理http请求和响应的
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,IOptions<AppSetting> options)
        {
            app.Run(async context => 
            {
                //通过读取配置方法，如果你有很多的
                var conStr = _configuration["ConnectionString"];
                var title = _configuration["WebSetting:Title"];

                //绑定配置模型对象参数
                var appSetting = new AppSetting();
                _configuration.Bind(appSetting);//全部绑定

                var webSeting = new WebSetting(); //部分绑定
                _configuration.GetSection("WebSetting").Bind(webSeting);

                //注册配置选项服务
                var a =options.Value.ConnectionString;



            });

            //封装中间件 
            //中间件 Use有next委托
            app.Use(async (context,next)=> 
            {
                await context.Response.WriteAsync("Middleware 1 begin\r\n");
                await next();
                await context.Response.WriteAsync("Middleware 1 end\r\n");
            });


            app.UseMiddleware<Test_Middleware>();
            app.UseTest();


            //Run 没有next，添加终端中间件
            //专门用来短路请求管道，是放在最后面的 兜底的
            app.Run(async context =>
            {
                await context.Response.WriteAsync("Hello Run\r\n");
            });


            //判断环境名称 Development
            if (env.IsDevelopment())
            {
                //开发人员异常页面
                app.UseDeveloperExceptionPage();
            }
            //终结点（端点）路由中间件
            //ASP.NET CORE 2.X 无路由配置
            //ASP.NET CORE 3.X 拆出来了 都有对路由的需求，所以拆出来了，为了复用！
            app.UseRouting();
            //还可以添加一些其他中间件
            //终结点中间件 这里是配置 配置中间件和路由的之间关系 映射
            //终结点你可以简单的理解为MVC, /控制器/action
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello World!");
                });
            });
        }
    }
}
