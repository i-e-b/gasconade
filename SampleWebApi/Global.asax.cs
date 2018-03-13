﻿using System;
using System.Web.Http;

namespace SampleWebApi
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            GlobalConfiguration.Configure(SwaggerConfig.Register);
            GlobalConfiguration.Configure(GasconadeConfig.Register);
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            var exc = Server.GetLastError();
            Response.Write("<h2>Diagnostic:</h2>\n");
            Response.Write("<p>" + exc.Message + "</p>\n");
            Response.Write("<pre>"+exc.StackTrace+"</pre>");
            Response.Flush();
            Response.End();
        }
    }

}
