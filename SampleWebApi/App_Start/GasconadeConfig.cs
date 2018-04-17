using System.Web.Http;
using ExternalLogDefinitions;
using Gasconade.UI;
using Tag;

namespace SampleWebApi
{
    public class GasconadeConfig {
        public static void Register(HttpConfiguration config) {
            config.EnableGasconadeUi(c =>
                {
                    c.AddAssembly(typeof(InsultMessage).Assembly);
                    c.AddSwaggerLink();
                    c.AddHeaderHtml(T.g("p")[
                        "This is a ", T.g("b")["sample"], " of Gasconade output, Running under .Net Framework MVC. Looks like it's working."
                    ]);
                });
        }

    }
}