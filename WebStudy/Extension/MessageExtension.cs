using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace WebStudy.Extension
{
    public static class MessageExtension
    {
        public static void AddMessages(this IServiceCollection services, Action<MessageBuilde> configre) 
        {
            //services.AddSingleton<>();
            var builder = new MessageBuilde(services);
            configre(builder);
        }
    }
}
