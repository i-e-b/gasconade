using System.Web.Http;
using ExternalLogDefinitions;
using Gasconade.UI;

namespace SampleWebApi
{
    public class GasconadeConfig {
        public static void Register(HttpConfiguration config) {
            config.EnableGasconadeUi(c =>
                {
                    c.AddAssembly(typeof(InsultMessage).Assembly);
                    c.AddSwaggerLink();
                });
        }

    }
}