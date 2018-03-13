using System.Web.Http;
using Gasconade.UI;
using Swashbuckle.Application;

namespace SampleWebApi
{
    public class SwaggerConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config
                .EnableSwagger(c =>
                    {
                        c.SingleApiVersion("v1", "SampleWebApi")
                            .Description("A sample web application.<br/>For logging details, see " + GasconadeConfig.Link("here"));
                    })
                .EnableSwaggerUi(c =>
                    {
                        c.DocumentTitle("My Swagger UI");
                    });
        }

    }
}
