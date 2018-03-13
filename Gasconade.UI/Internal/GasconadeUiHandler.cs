using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tag;

namespace Gasconade.UI.Internal
{
    public class GasconadeUiHandler : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var all = GasconadeConfig.KnownLogTypes().ToList();

            if ( ! all.Any()) {
                return TagHttpPage(EmptyLogsPage());
            }

            return TagHttpPage(ListingDocument(all));
        }

        public static TagContent ListingDocument(List<Type> all)
        {
            var lines = T.g();
            lines.Add(T.g("i")["Gasconade "]);
            if (! string.IsNullOrWhiteSpace(GasconadeConfig.ReturnLink)) {
                lines.Add(T.g("a", "href",GasconadeConfig.ReturnLink)["Return to API docs"]);
            }
            lines.Add(T.g("h1")["Log Documentation"]);

            foreach (var type in all)
            {
                var block = T.g("div", "class","MessageBlock");
                block.Add(T.g("h3")[type.Name]);
                if ( ! type.IsSubclassOf(typeof(TemplatedLogMessage))) {
                    block.Add(T.g("p")[
                        T.g("b")["Note: "],
                        "This type does not derive from ",
                        T.g("code")["Gasconade.TemplatedLogMessage"],
                        " and can't be sent"
                    ]);
                }

                var tmpl = TemplatedLogMessage.GetTemplateText(type);
                var desc = TemplatedLogMessage.GetDescription(type);
                var props = TemplatedLogMessage.GetPropertyDescriptions(type);

                if (string.IsNullOrWhiteSpace(tmpl)) {
                    block.Add(T.g("i")["This message has no template, and will be given a default message"]);
                } else {
                    block.Add(T.g("code", "class","template")[tmpl]);
                }

                if ( ! string.IsNullOrWhiteSpace(desc.Description)) {
                    block.Add(T.g("h4")["Description"]);
                    block.Add(T.g("p", "class","informative")[desc.Description]);
                }

                if ( ! string.IsNullOrWhiteSpace(desc.Causes)) {
                    block.Add(T.g("h4")["Causes"]);
                    block.Add(T.g("p", "class","informative")[desc.Causes]);
                }

                if ( ! string.IsNullOrWhiteSpace(desc.Actions)) {
                    block.Add(T.g("h4")["Actions"]);
                    block.Add(T.g("p", "class","informative")[desc.Actions]);
                }

                if (props.Count > 0) {
                    block.Add(T.g("h4")["Properties"]);
                    var defs = T.g("dl");
                    foreach (var prop in props)
                    {
                        defs.Add(T.g("dt")[prop.Key]);
                        defs.Add(T.g("dd")[prop.Value]);
                    }
                    block.Add(defs);
                }

                lines.Add(block);
            }

            return lines;
        }

        /// <summary>
        /// A page noting that no log messages were found
        /// </summary>
        private TagContent EmptyLogsPage()
        {
            return 
                T.g("body")[
                    T.g("h1")["No logs found"],
                    T.g("p")["This site either does not have any logs, or Gasconade has not been correctly configured"],
                    T.g("p")["Check you have called 'GasconadeConfig.AddAssembly(typeof(AMessageType).Assembly);' "]
                ] ;
        }

        private static Task<HttpResponseMessage> TagHttpPage(TagContent body)
        {
            if (body.Tag != "body") body = T.g("body")[body];


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

        private static string GetStyleData()
        {
            if ( ! string.IsNullOrWhiteSpace(GasconadeConfig.StylesheetText)) return GasconadeConfig.StylesheetText;

            return @"
.template {
    padding: 1em 2em;
    font-weight: bold;
    font-family: monospace;
    background-color: #ddf;
    display: block;
}
.MessageBlock:nth-child(odd) {
    background-color: #fff;
    margin: 10;
}
.MessageBlock:nth-child(even) {
    background-color: #eee;
    margin: 10;
}
h1, h2, h3, h4 {
    margin: 1em 0 0.5em;
    line-height: 1.25;
}
h3 {
    background-color: #ddd;
}
body {
    margin: 0;
    padding: 0;
    font-size: 100%;
    line-height: 1.5;
    font-family: sans-serif;
}
div { display: block; }
dt { font-weight: bold; font-size: 80%; }
dd { font-size: 80%; }
";
        }
    }
}
