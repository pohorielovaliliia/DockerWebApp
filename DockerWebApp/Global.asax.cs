using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;

namespace DockerWebApp
{
    public class Global : HttpApplication
    {
        public static ILogger<Global> Logger;

        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // Create a LoggerFactory and configure Console logging
            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
            });

            // Assign a logger to the Global class
            Logger = loggerFactory.CreateLogger<Global>();

            Logger.LogInformation("Application started.");
        }
    }
}




