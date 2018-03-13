using System;
using System.Web.Http;
using ExternalLogDefinitions;
using Gasconade.UI;
using SampleWebApi.LogMessages;

namespace SampleWebApi
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            GlobalConfiguration.Configure(SwaggerConfig.Register);
            GlobalConfiguration.Configure(GasconadeConfig.Register);        // this adds the Gasconade UI, and any messages in the calling assembly
            
            // TODO: fluent config like swagger
            GasconadeConfig.AddAssembly(typeof(InternalErrorMessage).Assembly);                // this adds messages in the current assembly
            GasconadeConfig.AddAssembly(typeof(InsultMessage).Assembly);    // this adds external Assembly's log messages
            GasconadeConfig.AddSwaggerLink();
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
