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
    /// ����webӦ������ķ�����м��
    /// </summary>
    public class Startup
    {
        private readonly IConfiguration _configuration;
        public Startup(IConfiguration configuration) 
        {
            _configuration = configuration;
        }


        /// <summary>
        /// ע�����
        /// </summary>
        /// <param name="services"></param>
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //ע������ѡ��ķ���
            services.Configure<AppSetting>(_configuration);
            //�������� 
            //Ioc(���Ʒ�ת)������ע������  ����ʵ��
            //��Ҫ��ʲô����ȥ�á�ʲô�� ��ת  ��Ҫ��ʲô��������� ��ת  ��ʲô�����������ѡ�ʲô�����㣩
            //����ע��:һϵ�й��ߺ��ֶ� ����Ŀ��������� ��ά�� �ɲ��ԵĴ���ͳ��򡣣���
            //����������ϵ�������IOC�����У�Ȼ�������������ʵ��

            //IOC���� ��ת�����ͽӿڷ�ʽ ֱ�ӲٿصĶ������Ȩ���˵�����ͨ��������ʵ�� ���������ת���͹���

            //һ��������Ĺ��� ����������
            //IOC����������   ����ṩ  ӳ������ �������Ĵ�����������

            //IOC�������ǼǴ���������Ƕ��� ע������ ���ܽ��� ĳ�����������Ķ���

            //����ע���� Unity Autofac Ninject ����.NET��

            //��ӿ�������API��ع��ܵ�֧�֣�����֧����ͼ��ҳ��
            //�������������� 
            services.AddControllers();
            //��ӿ�������API��ع��ܵ�֧��,֧����ͼ��ҳ��  ASP.NET CORE 3.X Ĭ��ģ��
            services.AddControllersWithViews();
            //��Ӷ�Razor Page����С��������֧��
            services.AddRazorPages();
            //ASP.NET CORE 2.x
            services.AddMvc();
            //����
            services.AddCors();
            //���÷��� Ҳ������� ����������(EF Swagger) 

            //ע���Զ������
            //������������   ������������

            //ע���Զ�������ʱ�� ����ѡ����������

            //˲ʱ ÿ�δӷ����������������ʵ��ʱ ���ᴴ��һ���µ�ʵ��
            //������ �̵߳��� ��ͬһ���̣߳������� ֵʵ����һ��
            //���� ȫ�ֵ��� ֻ�ᴴ��һ�� ÿһ�ζ�ʹ����ͬ��ʵ��
            // services.AddTransient()
            // services.AddScoped()
            // services.AddSingleton()
            //ע���� ���һ����Ч
            //���� ������ô�� ���ĸ���������
            //�Զ�����չ
            //services.AddMessages();
            //�Զ������
            //services.AddMessages(builder=> builder.UseEmail());

        }

        

        /// <summary>
        /// �����м��,�м����ɹܵ�
        /// ����� �м�����Ǵ���http�������Ӧ��
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,IOptions<AppSetting> options)
        {
            app.Run(async context => 
            {
                //ͨ����ȡ���÷�����������кܶ��
                var conStr = _configuration["ConnectionString"];
                var title = _configuration["WebSetting:Title"];

                //������ģ�Ͷ������
                var appSetting = new AppSetting();
                _configuration.Bind(appSetting);//ȫ����

                var webSeting = new WebSetting(); //���ְ�
                _configuration.GetSection("WebSetting").Bind(webSeting);

                //ע������ѡ�����
                var a =options.Value.ConnectionString;



            });

            //��װ�м�� 
            //�м�� Use��nextί��
            app.Use(async (context,next)=> 
            {
                await context.Response.WriteAsync("Middleware 1 begin\r\n");
                await next();
                await context.Response.WriteAsync("Middleware 1 end\r\n");
            });


            app.UseMiddleware<Test_Middleware>();
            app.UseTest();


            //Run û��next������ն��м��
            //ר��������·����ܵ����Ƿ��������� ���׵�
            app.Run(async context =>
            {
                await context.Response.WriteAsync("Hello Run\r\n");
            });


            //�жϻ������� Development
            if (env.IsDevelopment())
            {
                //������Ա�쳣ҳ��
                app.UseDeveloperExceptionPage();
            }
            //�ս�㣨�˵㣩·���м��
            //ASP.NET CORE 2.X ��·������
            //ASP.NET CORE 3.X ������� ���ж�·�ɵ��������Բ�����ˣ�Ϊ�˸��ã�
            app.UseRouting();
            //���������һЩ�����м��
            //�ս���м�� ���������� �����м����·�ɵ�֮���ϵ ӳ��
            //�ս������Լ򵥵����ΪMVC, /������/action
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
