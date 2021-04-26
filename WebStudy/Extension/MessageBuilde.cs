using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebStudy.Extension
{
    public class MessageBuilde
    {
        public IServiceCollection services;
        public MessageBuilde(IServiceCollection serviceCollection)
        {
            services = serviceCollection;
        }

        public void UseEmail()
        {

        }

        public void UsePhone() 
        {

        }

    }
}
