using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.Http;
using qwe.Utilities;

namespace qwe
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);            
            
            // Log application startup
            Logger.Info("Application started successfully", "Global");
        }

        void Application_Error(object sender, EventArgs e)
        {
            // Get the exception that caused the error
            Exception exception = Server.GetLastError();

            if (exception != null)
            {
                // Log the error
                Logger.Error("Unhandled application error", exception, "Global");

                // Clear the error to prevent default error page
                Server.ClearError();

                // Redirect to error page
                Response.Redirect("~/Home/Error");
            }
        }

        void Application_End(object sender, EventArgs e)
        {
            // Log application shutdown
            Logger.Info("Application shutting down", "Global");
        }
    }
}