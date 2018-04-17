using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Gasconade.UiCore;
using Tag;

namespace Gasconade.UI.Internal
{
    /// <summary>
    /// HTTP handler to show the Gasconade UI
    /// </summary>
    public class GasconadeUiHandler : HttpMessageHandler
    {
        /// <summary>
        /// Core handler
        /// </summary>
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var all = GasconadeUi.KnownLogTypes().ToList();

            if ( ! all.Any()) {
                return TagHttpPage(PageContentGenerator.EmptyLogsPage());
            }

            return TagHttpPage(PageContentGenerator.ListingDocument(all, GasconadeUi.ReturnLink, GasconadeUi.HeaderHtml));
        }

        private static Task<HttpResponseMessage> TagHttpPage(TagContent body)
        {
            if (body.Tag != "body") body = T.g("body")[body];

            body.Add(T.g("script", "type","text/javascript")[GetScriptData()]);

            var content = T.g("html")[
                T.g("head")[
                    T.g("title")["Log documentation (Gasconade)"],
                    T.g("style")[GetStyleData()]
                ],
                body
            ];

            var sampleMessage = new HttpResponseMessage(HttpStatusCode.OK) {
                Content = new StringContent(content, Encoding.UTF8, "text/html")
            };
            return Task.FromResult(sampleMessage);
        }

        private static string GetScriptData()
        {
            if ( ! string.IsNullOrWhiteSpace(GasconadeUi.ScriptText)) return GasconadeUi.ScriptText;
            return DefaultContent.DefaultJavaScript;
        }

        private static string GetStyleData()
        {
            if ( ! string.IsNullOrWhiteSpace(GasconadeUi.StylesheetText)) return GasconadeUi.StylesheetText;
            return DefaultContent.DefaultStyleSheet;
        }
    }
}
