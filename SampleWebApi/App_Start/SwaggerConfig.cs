using System.Web.Http;
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
                        c.SingleApiVersion("v1", "SampleWebApi");
                    })
                .EnableSwaggerUi(c =>
                    {
                        c.DocumentTitle("My Swagger UI");
                    });
        }

    }
}
