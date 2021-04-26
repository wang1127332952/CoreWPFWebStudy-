using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebStudy
{
    public class AppSetting
    {
        public string ConnectionString { get; set; }

        public WebSetting WebSetting { get; set; }
    
    }

    public class WebSetting
    {
        public string Title { get; set; }
        public Seting Seting { get; set; }
    }

    public class Seting 
    {
        public string name { set; get; }
    }
}
