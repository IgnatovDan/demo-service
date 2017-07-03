using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using Owin;
using System.Web.OData.Extensions;


[assembly: OwinStartup(typeof(Demo.Service.Startup))]

namespace Demo.Service
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            System.Web.Http.GlobalConfiguration.Configuration.EnableDependencyInjection();
            ConfigureAuth(app);
        }
    }
}
