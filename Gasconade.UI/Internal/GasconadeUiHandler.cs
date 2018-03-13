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
            var all = GasconadeUi.KnownLogTypes().ToList();

            if ( ! all.Any()) {
                return TagHttpPage(EmptyLogsPage());
            }

            return TagHttpPage(ListingDocument(all));
        }

        /// <summary>
        /// Create a html body that lists out all the known log message types with descriptions
        /// </summary>
        public static TagContent ListingDocument(List<Type> all)
        {
            var lines = T.g();
            lines.Add(T.g("i")["Gasconade "]);
            if (! string.IsNullOrWhiteSpace(GasconadeUi.ReturnLink)) {
                lines.Add(T.g("a", "href",GasconadeUi.ReturnLink)["Return to API docs"]);
            }
            lines.Add(T.g("h1")["Log Documentation"]);

            var sorted = all.Select(ReadTemplateMetadata).OrderBy(m=>m.Description.IsObsolete).ThenBy(m=>m.Name);

            foreach (var info in sorted)
            {
                var block = T.g("div", "class","MessageBlock");

                AddTitle(info, block);
                AddTemplate(info, block);
                block.Add(BuildDetailsExpander(info));

                lines.Add(block);
            }

            return lines;
        }

        private static void AddTemplate(TemplateMetadata info, TagContent block)
        {
            if (string.IsNullOrWhiteSpace(info.Template))
            {
                block.Add(T.g("i")["This message has no template, and will be given a default message"]);
            }
            else if (info.Description.IsObsolete)
            {
                block.Add(T.g("code", "class", "obsoleteTemplate")[info.Template]);
            }
            else
            {
                block.Add(T.g("code", "class", "template")[info.Template]);
            }
        }

        private static void AddTitle(TemplateMetadata info, TagContent block)
        {
            if (!string.IsNullOrWhiteSpace(info.Description.RetirementMessage))
            {
                block.Add(T.g("p", "class", "titleNote")[info.Description.RetirementMessage]);
            }

            block.Add(T.g("h3", "class", "header", "id", info.FullName)[info.Name]);
            if (!info.IsCorrectHierarchy)
            {
                AddSubtypeWarning(block);
            }
        }

        private static TemplateMetadata ReadTemplateMetadata(Type type)
        {
            return new TemplateMetadata{
                Name = type.Name,
                FullName = type.FullName,
                IsCorrectHierarchy = type.IsSubclassOf(typeof(TemplatedLogMessage)),
                Description = TemplatedLogMessage.GetDescription(type),
                Properties = TemplatedLogMessage.GetPropertyDescriptions(type),
                Template = TemplatedLogMessage.GetTemplateText(type)
            };
        }

        private static TagContent BuildDetailsExpander(TemplateMetadata info)
        {
            var expando = T.g("div",  "class","expando",  "id","expand_" + info.FullName);
            if (!string.IsNullOrWhiteSpace(info.Description.Description))
            {
                expando.Add(T.g("h4")["Description"]);
                expando.Add(T.g("p", "class", "informative")[info.Description.Description]);
            }

            if (!string.IsNullOrWhiteSpace(info.Description.Causes))
            {
                expando.Add(T.g("h4")["Causes"]);
                expando.Add(T.g("p", "class", "informative")[info.Description.Causes]);
            }

            if (!string.IsNullOrWhiteSpace(info.Description.Actions))
            {
                expando.Add(T.g("h4")["Actions"]);
                expando.Add(T.g("p", "class", "informative")[info.Description.Actions]);
            }

            if (info.Properties.Count > 0)
            {
                expando.Add(T.g("h4")["Properties"]);
                var defs = T.g("dl");
                foreach (var prop in info.Properties)
                {
                    defs.Add(T.g("dt")[prop.Key]);
                    defs.Add(T.g("dd")[prop.Value]);
                }

                expando.Add(defs);
            }

            return expando;
        }

        private static void AddSubtypeWarning(TagContent block)
        {
            block.Add(T.g("p")[
                T.g("b")["Note: "],
                "This type does not derive from ",
                T.g("code")["Gasconade.TemplatedLogMessage"],
                " and can't be sent"
            ]);
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
                    T.g("p")["Check you have called 'GasconadeUi.AddAssembly(typeof(AMessageType).Assembly);' "]
                ] ;
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

            return @"
function blockToggle(mouseEvt) {
    var trg = document.getElementById('expand_' + mouseEvt.target.id);
    trg.style.display = (trg.style.display == 'block') ? 'none' : 'block';
}
(function (){
    var all = document.getElementsByClassName('header');
    for(var i=0; i<all.length;i++){
        all[i].onclick = blockToggle;  
    }
})();
";
        }

        private static string GetStyleData()
        {
            if ( ! string.IsNullOrWhiteSpace(GasconadeUi.StylesheetText)) return GasconadeUi.StylesheetText;

            return @"
.template {
    padding: 1em 2em;
    font-weight: bold;
    font-family: monospace;
    background-color: #ddf;
    display: block;
}
.obsoleteTemplate {
    padding: 1em 2em;
    font-family: monospace;
    background-color: #fdd;
    display: block;
}
.expando {
    display:none;
}
.titleNote {
    float: right;
    margin-right: 1em;
}
.MessageBlock:nth-child(odd) {
    background-color: #fff;
    margin: 10;
}
.MessageBlock:nth-child(even) {
    background-color: #eee;
    margin: 10;
}
h1, h2, h4 {
    margin: 1em 0 0.5em;
    line-height: 1.25;
}
h3 {
    padding: 0.5em;
    margin: 0;
    background-color: #ddd;
}
h3:hover { background-color: #ccc; }
h3:active { background-color: #bbb; }
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
